namespace CustomDSATrainer.Domain.Interfaces.Services
{
    public interface ISubmissionService
    {
        void RunSumbission(Submission submission, string _inputs, string _outputs);
        void SaveToDatabase(Submission submission);
    }
}
