using CustomDSATrainer.Shared;
using Microsoft.AspNetCore.Authentication;


namespace CustomDSATrainer.Application
{
    public static class CodeReviewer
    {
        private static string reviewUnsolvedLLMPrompt = "AIService/CodeReview/Prompts/Prompt-Unsolved.txt";
        private static string pathToUnsolvedLLMReview = "AIService/CodeReview/Result.txt";
        public static string? ReviewUnsolvedProblem(string pathToSource)
        {
            string? review = null;

            if (SharedValues.CurrentActiveProblem == null) { return review; }

            string? problemStatement = SharedValues.CurrentActiveProblem.Statement;
            if (problemStatement == null) { return review; }

            string userSource = File.ReadAllText(Path.GetFullPath(pathToSource));
            string[] prompt = File.ReadAllLines(reviewUnsolvedLLMPrompt);

            prompt[3] = problemStatement;
            prompt[6] = userSource;

            File.WriteAllLines(Path.GetFullPath(reviewUnsolvedLLMPrompt), prompt);
            
            PythonAIService.ReviewUnsolvedPrompt();

            return File.ReadAllText(Path.GetFullPath(pathToUnsolvedLLMReview));
        }
    }
}
