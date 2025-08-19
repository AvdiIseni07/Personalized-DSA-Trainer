namespace CustomDSATrainer.Domain.Interfaces.Repositories
{
    public interface ISubmissionRepository
    {
        void SaveToDatabase(Submission submission);
    }
}
