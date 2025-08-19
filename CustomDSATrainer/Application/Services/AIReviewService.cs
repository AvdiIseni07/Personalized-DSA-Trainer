using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Interfaces.Repositories;
using CustomDSATrainer.Domain.Interfaces.Services;

namespace CustomDSATrainer.Application.Services
{
    public class AIReviewService : IAIReviewService
    {
        private readonly IAIReviewRepository _aiReviewRepository;
        public AIReviewService(IAIReviewRepository aIReviewRepository)
        {
            _aiReviewRepository = aIReviewRepository ?? throw new ArgumentNullException("AIReview repository cannot be null.");
        }
        public void SaveToDatabase(AIReview aiReview)
        {
            _aiReviewRepository.SaveToDatabase(aiReview);
        }
    }
}
