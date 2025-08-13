using CustomDSATrainer.Shared;
using Microsoft.AspNetCore.Authentication;


namespace CustomDSATrainer.Application
{
    public static class CodeReviewer
    {
        private static string reviewUnsolvedLLMPrompt = "AIService/CodeReview/Prompts/Prompt-Unsolved.txt";
        private static string reviewSolvedLLMPrompt = "AIService/CodeReview/Prompts/Prompt-Solved.txt";
        private static string AiReviewResult = "AIService/CodeReview/Result.txt";

        private static string solvedReviewTemplate = "AIService/CodeReview/Templates/Solved.txt";
        private static string unsolvedReviewTemplate = "AIService/CodeReview/Templates/Unsolved.txt";

        public static string? ReviewUnsolvedProblem(string pathToSource)
        {
            string? review = null;

            if (SharedValues.CurrentActiveProblem == null) { return review; }

            string? problemStatement = SharedValues.CurrentActiveProblem.Statement;
            if (problemStatement == null) { return review; }

            string userSource = File.ReadAllText(Path.GetFullPath(pathToSource));
            string[] prompt = File.ReadAllLines(unsolvedReviewTemplate);

            prompt[3] = problemStatement;
            prompt[6] = userSource;

            File.WriteAllLines(Path.GetFullPath(reviewUnsolvedLLMPrompt), prompt);
            
            PythonAIService.ReviewUnsolvedPrompt();
            review = File.ReadAllText(Path.GetFullPath(AiReviewResult));

            return review;
        }

        public static string? ReviewSolvedProblem(string pathToSource)
        {
            string? review = null;
            if (SharedValues.CurrentActiveProblem == null) { return review; }

            string? problemStatement = SharedValues.CurrentActiveProblem.Statement;
            if (problemStatement == null) { return review; }

            string userSource = File.ReadAllText(Path.GetFullPath(pathToSource));
            string[] prompt = File.ReadAllLines(Path.GetFullPath(solvedReviewTemplate));

            prompt[3] = problemStatement;
            prompt[6] = userSource;

            File.WriteAllLines(Path.GetFullPath(reviewSolvedLLMPrompt), prompt);

            PythonAIService.ReviewSolvedPromblem();
            review = File.ReadAllText(Path.GetFullPath(AiReviewResult));

            return review;
        }
    }
}
