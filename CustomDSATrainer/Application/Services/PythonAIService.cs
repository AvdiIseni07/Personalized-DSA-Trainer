using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Interfaces.Services;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace CustomDSATrainer.Application.Services
{
    /// <summary>
    /// Provides functionality for everything that has to do with the python scripts:
    /// <list type="bullet">
    /// <item>Generating a problem from prompt</item>
    /// <item>Generating a problem from unsolved</item>
    /// <item>Problem AI Review</item>
    /// </list>
    /// </summary>
    public class PythonAIService : IPythonAIService
    {
        private readonly string _pathToPythonProblemGen;
        private readonly string _pathToPythonUnsolvedReview;
        private readonly string _pathToPythonSolvedReview;

        private readonly ILogger<PythonAIService> _logger;

        private static int TIMEOUT_LIMIT = 60000; // in milliseconds

        public PythonAIService(IConfiguration configuration, ILogger<PythonAIService> logger)
        {
            _pathToPythonProblemGen = configuration["PythonData:ProblemGenerator"] ?? throw new Exception("Problem generator path cannot be null.");
            _pathToPythonSolvedReview = configuration["PythonData:SolvedReview"]    ?? throw new Exception("SolvedReview path cannot be null");
            _pathToPythonUnsolvedReview = configuration["PythonData:UnsolvedReview"] ?? throw new Exception("UnsolvedReview path cannot be null.");
            _logger = logger;
        }

        /// <summary>
        /// Creates and returns a <see cref="ProcessStartInfo"/> to execute a given python script.
        /// </summary>
        /// <param name="script">The relative path of the script that needs to be executed.</param>
        /// <returns>The <see cref="ProcessStartInfo"/> for the given script.</returns>
        private ProcessStartInfo GetStartInfo(string script)
        {
            _logger.LogTrace("Getting start info for {pathToScript}", script);
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

        /// <summary>
        /// Executes the python script to generate a new <see cref="Problem"/> based on the given categories and difficulty.
        /// It then gathers all the data for the problem in a List and returns it.
        /// </summary>
        /// <param name="categories">The categories the generated problem should have.</param>
        /// <param name="difficulty">The difficulty the genereted problem should be.</param>
        /// <returns>The problem data for the generated <see cref="Problem"/>, if generated successfully.</returns>
        /// <exception cref="ArgumentNullException">The python script has not produced any output.</exception>
        public List<string> GenerateProblemFromPrompt(string categories, string difficulty)
        {
            _logger.LogInformation("Problem generator has been called.");
            ProcessStartInfo startInfo = GetStartInfo(_pathToPythonProblemGen);

            string adaptedLine = $"Task: Generate a competitive-programming (LeetCode/Codeforces) style problem about these techniques, data structures and algorithms: {categories}. The difficulty should be: {difficulty}";
            List<string> output = new List<string>();

            try
            {
                using (var process = new Process { StartInfo = startInfo })
                {
                    process.Start();

                    process.StandardInput.WriteLine(adaptedLine);
                    process.StandardInput.Close();

                    if (!process.WaitForExit(TIMEOUT_LIMIT))
                    {
                        process.Kill(true);
                      
                        throw new TimeoutException($"Python script timed out after {TIMEOUT_LIMIT} milliseconds.");
                    }

                    while (!process.StandardOutput.EndOfStream)
                    {
                        string line = process.StandardOutput.ReadLine() ?? throw new ArgumentNullException(nameof(line), "Output cannot be null.");
                        output.Add(line);
                    }

                    output[0] = output[0].Replace('@', '\n');
                    output.Add(categories);
                    output.Add(difficulty);

                    string error = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    if (!error.IsNullOrEmpty() || process.ExitCode != 0)
                    {
                        throw new Exception(error);

                    }
                }
            }
            catch (Exception e) { _logger.LogError(e, "Error while runnin python script."); }

            return output;
        }

        /// <summary>
        /// Gets all the categories (<see cref="Problem.Categories"/>) of the user's unsolved problems as well as a randomly selected difficulty.
        /// It then modifies the AI prompt to randomly select some of the categories.
        /// It then generates the problem data based on the parameters.
        /// </summary>
        /// <param name="categories">All the categories of the user's unsolved problems.</param>
        /// <param name="difficulty">The randomly selected difficulty.</param>
        /// <returns>The problem data for the generated problem, if generates successfully.</returns>
        public List<string> GenerateProblemFromUnsolved(string categories, string difficulty)
        {
            string adaptedLine = $"Task: Generate a competitive-programming (LeetCode/Codeforces) style problem about these techniques, data structures and algorithms: {categories}"
                                + $". You do not have to use all of them. Use what you think would make the most interesting problem and would help the user the most. The difficulty should be {difficulty}";

            return GenerateProblemFromPrompt(categories, difficulty);
        }

        /// <summary>
        /// Calls the python script to generate an AI Review for a given <see cref="Problem"/> and a given user-generated source code which aims to solve that problem.
        /// It calls a different script based on whether the problem has been solved or not.
        /// </summary>
        /// <param name="problemStatement">The selected problem's statement.</param>
        /// <param name="userSource">The user-generated source code.</param>
        /// <param name="solved">Whether the selected problem has been solved or not.</param>
        /// <returns>The generated review for the given problem and source code.</returns>
        public async Task<string?> ReviewProblem(string problemStatement, string userSource, bool solved)
        {
            ProcessStartInfo startInfo = solved ? GetStartInfo(_pathToPythonSolvedReview) : GetStartInfo(_pathToPythonUnsolvedReview);

            string? review = string.Empty;
            try
            {
                using (var process = new Process { StartInfo = startInfo })
                {
                    process.Start();

                    process.StandardInput.Write(problemStatement);
                    process.StandardInput.Write("----------");
                    process.StandardInput.Write(userSource);
                    process.StandardInput.Close();

                    if (!process.WaitForExit(TIMEOUT_LIMIT))
                    {
                        process.Kill(true);
                        throw new TimeoutException($"Review timed out after {TIMEOUT_LIMIT} milliseconds.");
                    }

                    review = await process.StandardOutput.ReadToEndAsync();

                    await process.WaitForExitAsync();
                }
            } catch (TimeoutException e){ _logger.LogError(e, "Review time out."); }

            _logger.LogCritical("Review is {rev}.", review);

            return review;
        }
    }
}
