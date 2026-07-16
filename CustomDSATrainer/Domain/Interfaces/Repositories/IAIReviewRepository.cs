namespace CustomDSATrainer.Domain.Interfaces.Repositories
{
    public interface IAIReviewRepository
    {
        Task SaveToDatabase(AIReview aiReview);
    }
}
