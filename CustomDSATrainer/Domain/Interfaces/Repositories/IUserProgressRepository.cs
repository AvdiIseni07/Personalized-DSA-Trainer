namespace CustomDSATrainer.Domain.Interfaces.Repositories
{
    public interface IUserProgressRepository
    {
        void UpdateProblemData(Problem problem, Submission submission);
    }
}
