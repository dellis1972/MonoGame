using System.Text.RegularExpressions;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Effect
{
    public static class GLSLManipulator
    {
        static char[] lineEnder = { '\n', '\0' };

        public static void RemoveVersionHeader(ref string glsl)
        {
            int version = glsl.IndexOf("#version");
            if (version >= 0)
            {
                int lineEnd = glsl.IndexOfAny(lineEnder, version);
                glsl = glsl.Remove(version, lineEnd - version);
            }
        }

        public static void RemoveARBSeparateShaderObjects(ref string glsl)
        {
            glsl = glsl.Replace("#extension GL_ARB_separate_shader_objects : require\n", "");
        }

        public static bool RemoveOutGlPerVertex(ref string glsl)
        {
            string gl_PerVertex = "\nout gl_PerVertex\n{\n    vec4 gl_Position;\n};\n";
            string glslNew = glsl.Replace(gl_PerVertex, "");

            if (glslNew == glsl)
                return false;

            glsl = glslNew;
            return true;
        }

        public static void RemoveInGlPerVertex(ref string glsl)
        {
            string gl_PerVertex = "\nin gl_PerVertex\n{\n    vec4 gl_Position;\n};\n";
            glsl = glsl.Replace(gl_PerVertex, "");
        }

        /// <summary>
        /// Fix SPIRV-Cross varying name mismatch between vertex and fragment shaders.
        /// SPIRV-Cross prefixes vertex shader outputs with "out_var_" and fragment shader
        /// inputs with "in_var_", but GLSL 330 matches inter-stage variables by name.
        /// This renames both to a common "mg_" prefix so they match at link time.
        /// </summary>
        /// <remarks>
        /// This is safe because:
        /// - Vertex shader inputs (in_var_*) use layout(location) and are matched by location, not name.
        /// - Fragment shader outputs (out_var_SV_Target*) use layout(location) and are matched by location, not name.
        /// So only the inter-stage varyings (vertex out_var_ / fragment in_var_) are affected.
        /// </remarks>
        public static void FixVaryingNames(ref string glsl, bool isVertexShader)
        {
            if (isVertexShader)
                glsl = glsl.Replace("out_var_", "mg_");
            else
                glsl = glsl.Replace("in_var_", "mg_");
        }

        /// <summary>
        /// Ensure pixel and vertex shaders have consistent precision qualifiers for WebGL/GLES.
        /// SPIRV-Cross output may lack precision qualifiers which causes linker errors like
        /// "number of uniform block differ between VERTEX and FRAGMENT shaders".
        /// This adds highp qualifiers to all float-based types (float, vecN, matN, matNxM).
        /// </summary>
        /// <param name="glsl">The GLSL source code to modify.</param>
        /// <param name="isGLES">True if targeting GLES/WebGL.</param>
        public static void InjectPrecision(ref string glsl, bool isGLES)
        {
            if (!isGLES)
                return;

            // Upgrade mediump to highp for consistency between vertex and fragment shaders
            glsl = glsl.Replace("precision mediump float;", "precision highp float;");

            // Add default precision statements after version directive if not present
            if (!glsl.Contains("precision highp float;"))
            {
                glsl = glsl.Replace("#version 300 es", "#version 300 es\nprecision highp float;\nprecision highp int;");
            }

            // Pattern for float-based types that need precision qualifiers
            const string floatTypes = @"float|vec[234]|mat[234](?:x[234])?";

            // Add highp to uniform block members with layout qualifiers (e.g., layout(row_major))
            // SPIRV-Cross generates: layout(row_major) mat4 Name;
            // We need: layout(row_major) highp mat4 Name;
            glsl = Regex.Replace(glsl,
                $@"(layout\s*\([^)]+\)\s+)(?!(highp|mediump|lowp)\s)({floatTypes})\b",
                "$1highp $3");

            // Add highp to uniform block members without layout qualifiers
            // These are indented lines inside uniform blocks: "    vec4 Color;"
            // Use multiline mode to match start of line with ^
            glsl = Regex.Replace(glsl,
                $@"^(\s+)(?!(highp|mediump|lowp|in|out|uniform|layout)\b)({floatTypes})\s+(\w+\s*[;\[=])",
                "$1highp $3 $4",
                RegexOptions.Multiline);
        }

        /// <summary>
        /// Strip layout(binding = N) qualifiers and the GL_ARB_shading_language_420pack
        /// ifdef block from GLSL source. macOS GL 4.1 doesn't support the 420pack extension,
        /// so these must be removed at compile time. The binding info is already stored in the
        /// bytecode header and applied at runtime via glUniformBlockBinding / glUniform1i.
        /// </summary>
        public static void StripBindingQualifiers(ref string glsl)
        {
            // Remove "binding = N" from layout qualifiers that have other qualifiers too
            // e.g. layout(binding = 0, std140) -> layout(std140)
            glsl = Regex.Replace(glsl, @"binding\s*=\s*\d+\s*,\s*", "");
            glsl = Regex.Replace(glsl, @",\s*binding\s*=\s*\d+", "");

            // Remove layout(binding = N) when binding is the only qualifier
            // e.g. layout(binding = 0) uniform -> uniform
            glsl = Regex.Replace(glsl, @"layout\s*\(\s*binding\s*=\s*\d+\s*\)\s*", "");

            // Remove the GL_ARB_shading_language_420pack ifdef block
            glsl = Regex.Replace(glsl, @"#ifdef GL_ARB_shading_language_420pack\s*\n.*?\n#endif\s*\n", "", RegexOptions.Singleline);
        }

        /// <summary>
        /// Rename UBO block and instance names to stage-specific names for WebGL/GLES.
        /// WebGL requires uniform blocks with the same name to have identical definitions
        /// across linked stages. When SpriteBatch links its VS with a custom PS-only effect,
        /// the type_MG_Globals blocks have different members and linking fails. Renaming the
        /// block type and instance per-stage avoids this collision.
        /// </summary>
        public static void RenameUniformBlock(ref string glsl, bool isVertexShader, bool isGLES)
        {
            // Always rename UBO blocks per-stage. SPIRV-Cross may strip unused members,
            // causing VS and PS blocks with the same name to have different definitions.
            // OpenGL requires uniform blocks with the same name to have identical definitions
            // across linked stages, so renaming avoids link failures.
            string suffix = isVertexShader ? "VS" : "PS";
            // Rename block type first (type_MG_Globals contains _MG_Globals as substring).
            // After this, the type name no longer contains _MG_Globals.
            glsl = glsl.Replace("type_MG_Globals", $"type_MG_UBO_{suffix}");
            // Then rename instance name and member access expressions.
            glsl = glsl.Replace("_MG_Globals", $"_MG_UBO_{suffix}");
        }

        public static void AddPosFixupUniformAndCode(ref string glsl, ShaderStage shaderStage)
        {      
            // make sure gl_Position is being used
            int mainShader = glsl.LastIndexOf("void main(");
            if (glsl.IndexOf("gl_Position =", mainShader) < 0)
                return;

            // Add posFixup parameter to the shader, so we can compensate for differences btw DirectX and OpenGL
            string posFixup = "uniform vec4 posFixup;";

            int cursor = glsl.LastIndexOf('#');
            if (cursor < 0)
                cursor = 0;
            else
                cursor = glsl.IndexOfAny(lineEnder, cursor);

            glsl = glsl.Insert(cursor, "\n" + posFixup);

            // Add posFixup code to the end of the shader.
            // OpenGL uses flipped y-coordinates when rendering to a render target, in this case posFixup.y will be -1.
            // posFixup.zw is for emulating the DX9 half-pixel-offset.
            // The final change to gl_Position.z is needed because OpenGL uses a -1..1 clipspace, while DX uses 0..1
            string posFixupCode =
            "    gl_Position.y = gl_Position.y * posFixup.y;\n" +
            "    gl_Position.xy += posFixup.zw * gl_Position.ww;\n" +
            "    gl_Position.z = gl_Position.z * 2.0 - gl_Position.w;\n";

            cursor = glsl.LastIndexOf('}');
            glsl = glsl.Insert(cursor, posFixupCode);
        }
    }
}