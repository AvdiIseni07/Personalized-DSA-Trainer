namespace CustomDSATrainer.Domain.Interfaces.Services
{
    public interface IProblemService
    {
        void SubmitProblem(Problem problem, string pathToExe);
        bool LoadProblem(int id);
        string? AiReview(Problem problem, string pathToSource);
        void SaveToDatabase(Problem problem);
    }
}
