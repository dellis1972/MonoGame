// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using MonoGame.Interop;


namespace Microsoft.Xna.Framework.Graphics;

partial class VertexInputLayout
{
    public void GenerateInputElements(VertexAttribute[] inputs, out MGG_InputElement[] elements, out int[] strides)
    {
        elements = new MGG_InputElement[inputs.Length];
        strides = new int[Count];

        var missingShaderInputs = false;

        // So we are looking for element matches between the REQUIRED vertex
        // shader inputs and the provided vertex declarations for this draw.
        //
        // We don't concern ourselves with the same vertex elements bound
        // to multiple shader inputs because that is useful behavior.
        //
        // We also don't worry about unused vertex declaration elements
        // as we expect the shader is what drives the rendering.  For example
        // you may use the same model data for rendering shadows which just
        // need a position and rendering lighting which need position and a normal.

        for (int i = 0; i < inputs.Length; i++)
        {
            var attr = inputs[i];

            bool found = false;

            for (int j = 0; j < Count; j++)
            {
                var declaration = VertexDeclarations[j];
                var vertexElements = declaration.InternalVertexElements;
                var instanceFrequencies = InstanceFrequencies[j];
                
                foreach (var vertexElement in vertexElements)
                {
                    if (vertexElement.VertexElementUsage == attr.usage &&
                        vertexElement.UsageIndex == attr.index)
                    {
                        found = true;
                        elements[i] = vertexElement.AsInputElement(j, instanceFrequencies);
                        strides[j] = declaration.VertexStride;
                        break;
                    }
                }

                if (found)
                    break;
            }

            if (!found)
                missingShaderInputs = true;
        }

        if (missingShaderInputs)
        {
            // Build a string listing elements actually present in the vertex declaration(s),
            // using the same HLSL semantic names that the shader expects.
            var sb = new System.Text.StringBuilder();
            for (int j = 0; j < Count; j++)
            {
                var vertexElements = VertexDeclarations[j].InternalVertexElements;
                foreach (var ve in vertexElements)
                {
                    if (sb.Length > 0)
                        sb.Append(", ");
                    sb.Append(VertexElementUsageToSemantic(ve.VertexElementUsage));
                    sb.Append(ve.UsageIndex);
                }
            }

            var message = "An error occurred while preparing to draw. "
                        + "This is probably because the current vertex declaration does not include all the elements "
                        + "required by the current vertex shader. The current vertex declaration includes these elements: "
                        + sb.ToString() + ".";

            throw new InvalidOperationException(message);
        }
    }

    private static string VertexElementUsageToSemantic(VertexElementUsage usage)
    {
        switch (usage)
        {
            case VertexElementUsage.Position: return "POSITION";
            case VertexElementUsage.Color: return "COLOR";
            case VertexElementUsage.Normal: return "NORMAL";
            case VertexElementUsage.TextureCoordinate: return "TEXCOORD";
            case VertexElementUsage.BlendIndices: return "BLENDINDICES";
            case VertexElementUsage.BlendWeight: return "BLENDWEIGHT";
            case VertexElementUsage.Binormal: return "BINORMAL";
            case VertexElementUsage.Tangent: return "TANGENT";
            case VertexElementUsage.PointSize: return "PSIZE";
            case VertexElementUsage.Depth: return "DEPTH";
            case VertexElementUsage.Fog: return "FOG";
            case VertexElementUsage.Sample: return "SAMPLE";
            case VertexElementUsage.TessellateFactor: return "TESSELLATEFACTOR";
            default: return usage.ToString().ToUpperInvariant();
        }
    }
}
