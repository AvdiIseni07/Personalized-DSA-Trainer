namespace CustomDSATrainer.Domain.Interfaces.Services
{
    public interface IProblemGeneratorService
    {
        Problem GenerateFromPrompt(string categories, string difficulty);
        Task<Problem?> GenerateProblemFromUnsolved();
        Task<Problem?> Revision();
        Task<Problem?> RevisionWithCategories(string categories);
    }
}
