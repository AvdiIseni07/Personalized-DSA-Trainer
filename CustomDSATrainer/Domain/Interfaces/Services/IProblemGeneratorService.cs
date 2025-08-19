namespace CustomDSATrainer.Domain.Interfaces.Services
{
    public interface IProblemGeneratorService
    {
        Problem GenerateFromPrompt(string categories, string difficulty);
        Problem? GenerateProblemFromUnsolved();
        Problem? Revision();
        Problem? RevisionWithCategories(string categories);
    }
}
