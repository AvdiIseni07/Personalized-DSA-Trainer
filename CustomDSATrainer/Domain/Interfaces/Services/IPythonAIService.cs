namespace CustomDSATrainer.Domain.Interfaces.Services
{
    public interface IPythonAIService
    {
        List<string> GenerateProblemFromPrompt(string categories, string difficulty);
        List<string> GenerateProblemFromUnsolved(string categories, string difficulty);
        string? ReviewProblem(string problemStatement, string userSource, bool solved);
    }
}
