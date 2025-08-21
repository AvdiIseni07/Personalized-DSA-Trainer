using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CustomDSATrainer.Persistance.Repositories
{
    /// <summary>
    /// Repository for <see cref="Submission"/>
    /// </summary>
    public class SubmissionRepository : ISubmissionRepository
    {
        private readonly IDbContextFactory<ProjectDbContext> _contextFactory;
        public SubmissionRepository(IDbContextFactory<ProjectDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        /// <summary>
        /// Saves a <see cref="Submission"/> to the database.
        /// If it already exists, it only updates the values.
        /// </summary>
        /// <param name="submission">The submission that needs to be saved.</param>
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
