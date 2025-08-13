using System.Diagnostics;

namespace CustomDSATrainer.Application
{
    public static class PythonAIService
    {
        public static string pathToPythonProblemGen = "AIService/ProblemGenerator.py";
        public static string pathToPythonUnsolvedReview = "AIService/UnsolvedCodeReviewer.py";
        private static string pathToPythonSolvedReview = "AIService/SolvedCodeReviewer.py";
        
        public static string pathToLLMPrompt = "AIService/ProblemLLMPrompt.txt";

        private static void RunPythonScript(string script)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = $"-u \"{Path.GetFullPath(script)}\"",
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

            RunPythonScript(pathToPythonProblemGen);
        }
        public static void GenerateProblemFromUnsolved(string categories)
        {
            string adaptedLine = "Task: Generate a competitive-programming (LeetCode/Codeforces) style problem about these techniques, data structures and algorithms: " + categories
                                + ". You do not have to use all of them. Use what you think would make the most interesting problem and would help the user the most.";

            string[] lines = File.ReadAllLines(Path.GetFullPath(pathToLLMPrompt));
            lines[0] = adaptedLine;

            File.WriteAllLines(Path.GetFullPath(pathToLLMPrompt), lines);

            RunPythonScript(pathToPythonProblemGen);
        }

        public static void ReviewUnsolvedPrompt()
        {
            RunPythonScript(pathToPythonUnsolvedReview);
        }

        public static void ReviewSolvedPromblem()
        {
            RunPythonScript(pathToPythonSolvedReview);
        }
    }
}
