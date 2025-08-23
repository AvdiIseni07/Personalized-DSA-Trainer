namespace CustomDSATrainer.Domain.Interfaces.Services
{
    public interface IAIReviewService
    {
        Task SaveToDatabase(AIReview aiReview);
    }
}
