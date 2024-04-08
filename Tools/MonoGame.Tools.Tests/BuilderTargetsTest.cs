using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using NUnit.Framework;
namespace MonoGame.Tests.ContentPipeline
{
    [TestFixture]
    public class BuilderTargetsTest
    {
        bool RunBuild(string buildTool, string projectFile, string workingDir, params string[] parameters)
        {
            var psi = new ProcessStartInfo(buildTool)
            {
                Arguments = "build " + projectFile + " -bl -t:IncludeContent " + string.Join(" ", parameters) + " /noconsolelogger \"/flp1:LogFile=build.log;Encoding=UTF-8;Verbosity=Diagnostic\"",
                WorkingDirectory = workingDir,
                UseShellExecute = true,
            };
            using (var process = Process.Start(psi))
            {
                process.WaitForExit();
                return process.ExitCode == 0;
            }
        }

        [Test]
        public void BuildSimpleProject()
        {
            var root = Path.GetDirectoryName(typeof(BuilderTargetsTest).Assembly.Location);
            var outputPath = Path.Combine(root, "Assets", "Projects", "Content", "bin");
            if (Directory.Exists(outputPath))
                Directory.Delete(outputPath, recursive: true);

            var result = RunBuild("dotnet", Path.Combine("Assets", "Projects", "BuildSimpleProject.csproj"), root);
            Assert.AreEqual(true, result, "Content Build should have succeeded.");
            var contentFont = Path.Combine(outputPath, "DesktopGL", "Content", "ContentFont.xnb");
            Assert.IsTrue(File.Exists(contentFont), "'" + contentFont + "' should exist.");
        }
    }
}
