using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CustomDSATrainer.Persistance.Repositories
{
    public class SubmissionRepository : ISubmissionRepository
    {
        private readonly IDbContextFactory<ProjectDbContext> _contextFactory;
        public SubmissionRepository(IDbContextFactory<ProjectDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async void SaveToDatabase(Submission submission)
        {
            using (var context = await _contextFactory.CreateDbContextAsync())
            {
                var existingSubmission = await context.Submissions.FindAsync(submission.Id);
                if (existingSubmission == null)
                {
                    context.Submissions.Add(submission);
                }
                else
                {
                    context.Entry(existingSubmission).CurrentValues.SetValues(submission);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
