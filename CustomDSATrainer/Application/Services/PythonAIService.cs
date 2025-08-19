using CustomDSATrainer.Domain.Interfaces.Services;
using System.Diagnostics;

namespace CustomDSATrainer.Application.Services
{
    public class PythonAIService : IPythonAIService
    {
        private readonly string pathToPythonProblemGen = "AIService/ProblemGenerator.py";
        private readonly string pathToPythonUnsolvedReview = "AIService/UnsolvedCodeReviewer.py";
        private readonly string pathToPythonSolvedReview = "AIService/SolvedCodeReviewer.py";

        private ProcessStartInfo GetStartInfo(string script)
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
        public List<string> GenerateProblemFromPrompt(string categories, string difficulty)
        {
            ProcessStartInfo startInfo = GetStartInfo(pathToPythonProblemGen);

            string adaptedLine = $"Task: Generate a competitive-programming (LeetCode/Codeforces) style problem about these techniques, data structures and algorithms: {categories}. The difficulty should be: {difficulty}";
            List<string> output = new List<string>();

            using (var  process = new Process { StartInfo = startInfo })
            {
                process.Start();

                process.StandardInput.WriteLine(adaptedLine);
                process.StandardInput.Close();

                while (!process.StandardOutput.EndOfStream)
                {
                    string line = process.StandardOutput.ReadLine() ?? throw new ArgumentNullException(nameof(line), "Output cannot be null.");
                    output.Add(line);
                }
                process.StandardOutput.Close();

                
                output[0] = output[0].Replace('@', '\n');
                output.Add(categories);
                output.Add(difficulty);

                process.WaitForExit();
            }

            return output;
        }
        public List<string> GenerateProblemFromUnsolved(string categories, string difficulty)
        {
            string adaptedLine = $"Task: Generate a competitive-programming (LeetCode/Codeforces) style problem about these techniques, data structures and algorithms: {categories}"
                                + $". You do not have to use all of them. Use what you think would make the most interesting problem and would help the user the most. The difficulty should be {difficulty}";

            return GenerateProblemFromPrompt(categories, difficulty);
        }

        public string ReviewProblem(string problemStatement, string userSource, bool solved)
        {
            ProcessStartInfo startInfo = solved ? GetStartInfo(pathToPythonSolvedReview) : GetStartInfo(pathToPythonUnsolvedReview);

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
