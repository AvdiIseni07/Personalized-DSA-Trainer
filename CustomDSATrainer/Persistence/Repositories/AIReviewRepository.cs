using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CustomDSATrainer.Persistance.Repositories
{
    /// <summary>
    /// Repository for <see cref="AIReview"/>
    /// </summary>
    public class AIReviewRepository : IAIReviewRepository
    {
        private readonly ProjectDbContext _context;
        public AIReviewRepository(ProjectDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context), "DbContext cannot be null.");
        }

        /// <summary>
        /// Save an <see cref="AIReview"/> to the database.
        /// If it already exists, it only updates the values.
        /// </summary>
        /// <param name="aiReview">The AIReview that needs to be saved.</param>
        public async void SaveToDatabase(AIReview aiReview)
        {
            var existingReview = await _context.AIReview.FindAsync(aiReview.Id);

            if (existingReview == null)
            {
                await _context.AIReview.AddAsync(aiReview);
            }
            else
            {
                _context.Entry(existingReview).CurrentValues.SetValues(aiReview);
            }
        }
    }
}
