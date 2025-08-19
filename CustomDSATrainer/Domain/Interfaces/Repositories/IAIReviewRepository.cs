namespace CustomDSATrainer.Domain.Interfaces.Repositories
{
    public interface IAIReviewRepository
    {
        void SaveToDatabase(AIReview aiReview);
    }
}
