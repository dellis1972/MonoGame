
namespace BuildScripts;

[TaskName("Build OpenGL 4 Shaders")]
[IsDependentOn(typeof(BuildMGFXCTask))]
public sealed class BuildShadersOGL4Task : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        var mgfxc = context.GetProjectPath(ProjectType.Tools, "MonoGame.Effect.Compiler");
        var shadersDir = "MonoGame.Framework/Platform/Graphics/Effect/Resources";
        var workingDir = "native/monogame/opengl/";

        foreach (var filePath in context.GetFiles($"{shadersDir}/*.fx"))
            {
                context.Information($"Building {filePath.GetFilename()}");
                context.DotNetRun(mgfxc, $"\"{filePath}\" {filePath.GetFilenameWithoutExtension()}.ogl.mgfxo.h /Profile:OpenGL4", workingDir);
                context.Information("");
            }
    }
}
