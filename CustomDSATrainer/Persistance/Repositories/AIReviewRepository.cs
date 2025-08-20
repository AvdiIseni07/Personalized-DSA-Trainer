using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CustomDSATrainer.Persistance.Repositories
{
    public class AIReviewRepository : IAIReviewRepository
    {
        private readonly IDbContextFactory<ProjectDbContext> _contextFactory;
        public AIReviewRepository(IDbContextFactory<ProjectDbContext> contextFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory), "ContextFactory cannot be null.");
        }

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
