using CustomDSATrainer.Domain;
using System.Diagnostics;

namespace CustomDSATrainer.Application
{
    public static class PythonAIService
    {
        private static string pathToPythonProblemGen = "AIService/ProblemGenerator.py";
        private static string pathToPythonUnsolvedReview = "AIService/UnsolvedCodeReviewer.py";
        private static string pathToPythonSolvedReview = "AIService/SolvedCodeReviewer.py";
        
        private static ProcessStartInfo GetStartInfo(string script)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = $"-u \"{Path.GetFullPath(script)}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            return startInfo;
        }
        public static List<string> GenerateProblemFromPrompt(string categories)
        {
            ProcessStartInfo startInfo = GetStartInfo(pathToPythonProblemGen);

            string adaptedLine = "Task: Generate a competitive-programming (LeetCode/Codeforces) style problem about these techniques, data structures and algorithms: " + categories;
            List<string> output = new List<string>();
            
            using (var process = new Process { StartInfo = startInfo }) 
            {
                process.Start();

                process.StandardInput.WriteLine(adaptedLine);
            
                while (!process.StandardOutput.EndOfStream)
                {
                    string line = process.StandardOutput.ReadLine();
                    output.Add(line);
                }

                output[0] = output[0].Replace('@', '\n');
                output.Add(categories);

                process.WaitForExit();
            }

            return output;
        }
        public static List<string> GenerateProblemFromUnsolved(string categories)
        {
            string adaptedLine = "Task: Generate a competitive-programming (LeetCode/Codeforces) style problem about these techniques, data structures and algorithms: " + categories
                                + ". You do not have to use all of them. Use what you think would make the most interesting problem and would help the user the most.";

            return GenerateProblemFromPrompt(categories);
        }

        public static string ReviewProblem(string problemStatement, string userSource, bool solved)
        {
            ProcessStartInfo startInfo = (solved) ? GetStartInfo(pathToPythonSolvedReview) : GetStartInfo(pathToPythonUnsolvedReview);

            string review = string.Empty;
            using (var process = new Process { StartInfo = startInfo })
            {
                process.Start();

                process.StandardInput.Write(problemStatement);
                process.StandardInput.Write("----------");
                process.StandardInput.Write(userSource);
                process.StandardInput.Close();

                review = process.StandardOutput.ReadToEnd();

                process.WaitForExit();
            }

            return review;
        }
    }
}
