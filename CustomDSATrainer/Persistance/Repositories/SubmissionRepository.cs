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
        public void SaveToDatabase(Submission submission)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var existingSubmission = context.Submissions.Find(submission.Id);
                if (existingSubmission == null)
                {
                    context.Submissions.Add(submission);
                }
                else
                {
                    context.Entry(existingSubmission).CurrentValues.SetValues(submission);
                }

                context.SaveChanges();
            }
        }
    }
}
