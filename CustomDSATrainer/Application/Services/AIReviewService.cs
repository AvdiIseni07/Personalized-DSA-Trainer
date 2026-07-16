using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Interfaces.Repositories;
using CustomDSATrainer.Domain.Interfaces.Services;
using CustomDSATrainer.Domain.Interfaces.UnitOfWork;
using System.Threading.Tasks;

namespace CustomDSATrainer.Application.Services
{
    public class AIReviewService : IAIReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AIReviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException("UnitOfWork cannot be null.");
        }

        /// <summary>
        /// Saves an <see cref="AIReview"/> to the database.
        /// </summary>
        /// <param name="aiReview"></param>
        public async Task SaveToDatabase(AIReview aiReview)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _unitOfWork.AIReviewRepository.SaveToDatabase(aiReview);
                await _unitOfWork.CommitAsync();
            }
            catch { await _unitOfWork.RollbackTransactionAsync(); }
        }
    }
}
