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
        private readonly IDbContextFactory<ProjectDbContext> _contextFactory;
        public AIReviewRepository(IDbContextFactory<ProjectDbContext> contextFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory), "ContextFactory cannot be null.");
        }

        /// <summary>
        /// Save an <see cref="AIReview"/> to the database.
        /// If it already exists, it only updates the values.
        /// </summary>
        /// <param name="aiReview">The AIReview that needs to be saved.</param>
        public async void SaveToDatabase(AIReview aiReview)
        {
            using (var context = await _contextFactory.CreateDbContextAsync())
            {
                var existingReview = await context.AIReview.FindAsync(aiReview.Id);

                if (existingReview == null)
                {
                    await context.AIReview.AddAsync(aiReview);
                }
                else
                {
                    context.Entry(existingReview).CurrentValues.SetValues(aiReview);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
