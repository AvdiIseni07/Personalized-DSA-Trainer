using Azure.Core.Pipeline;
using System.Diagnostics;

namespace CustomDSATrainer.Application
{
    public static class PythonAIService
    {
        public static string pathToPython = "AIService/main.py";
        public static string pathToLLMPrompt = "AIService/LLMPrompt.txt";

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
            process.WaitForExit();
        }
        public static void GenerateProblemFromPrompt(string categories)
        {
            string adaptedLine = "Task: Generate a competitive-programming (LeetCode/Codeforces) style problem about these techniques, data structures and algorithms: " + categories;

            string[] lines = File.ReadAllLines(Path.GetFullPath(pathToLLMPrompt));
            lines[0] = adaptedLine;

            File.WriteAllLines(Path.GetFullPath(pathToLLMPrompt), lines);

            RunPythonScript();
        }
    }
}
