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

        public void SaveToDatabase(AIReview aiReview)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var existingReview = context.AIReview.Find(aiReview.Id);

                if (existingReview == null)
                {
                    context.AIReview.Add(aiReview);
                }
                else
                {
                    context.Entry(existingReview).CurrentValues.SetValues(aiReview);
                }

                context.SaveChanges();
            }
        }
    }
}
