namespace CustomDSATrainer.Domain.Interfaces.Services
{
    public interface IProblemService
    {
        void SubmitProblem(Problem problem, string pathToExe);
        Task<bool> LoadProblem(int id);
        Task<string?> AiReview(Problem problem, string pathToSource);
        Task SaveToDatabase(Problem problem);
    }
}
