using Azure.Core.Pipeline;
using System.Diagnostics;

namespace CustomDSATrainer.Application
{
    public static class PythonAIService
    {
        public static string pathToPython = "AIService/main.py";

        private static void RunPythonScript()
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = $"-u \"{Path.GetFullPath(pathToPython)}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            var process = new Process { StartInfo = startInfo };
            process.Start();
        }

    }
}
