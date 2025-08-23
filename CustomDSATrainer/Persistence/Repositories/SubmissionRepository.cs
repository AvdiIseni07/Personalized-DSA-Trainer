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
        private readonly ProjectDbContext _context;
        public SubmissionRepository(ProjectDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context), "DbContext cannot be null.");
        }

        /// <summary>
        /// Saves a <see cref="Submission"/> to the database.
        /// If it already exists, it only updates the values.
        /// </summary>
        /// <param name="submission">The submission that needs to be saved.</param>
        public async void SaveToDatabase(Submission submission)
        {
            var existingSubmission = await _context.Submissions.FindAsync(submission.Id);
            if (existingSubmission == null)
            {
                _context.Submissions.Add(submission);
            }
            else
            {
                _context.Entry(existingSubmission).CurrentValues.SetValues(submission);
            }
        }
    }
}
