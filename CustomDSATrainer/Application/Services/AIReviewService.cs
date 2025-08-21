using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Interfaces.Repositories;
using CustomDSATrainer.Domain.Interfaces.Services;

namespace CustomDSATrainer.Application.Services
{
    public class AIReviewService : IAIReviewService
    {
        private readonly IAIReviewRepository _aiReviewRepository;
        public AIReviewService(IAIReviewRepository aiReviewRepository)
        {
            _aiReviewRepository = aiReviewRepository ?? throw new ArgumentNullException("AIReview repository cannot be null.");
        }

        /// <summary>
        /// Saves an <see cref="AIReview"/> to the database.
        /// </summary>
        /// <param name="aiReview"></param>
        public void SaveToDatabase(AIReview aiReview)
        {
            _aiReviewRepository.SaveToDatabase(aiReview);
        }
    }
}
