using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CustomDSATrainer.Persistance.Repositories
{
    /// <summary>
    /// Repository for <see cref="UserProgress"/>.
    /// </summary>
    public class UserProgressRepository : IUserProgressRepository
    {
        private readonly IDbContextFactory<ProjectDbContext> _contextFactory;
        public UserProgressRepository (IDbContextFactory<ProjectDbContext> contextFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory), "ContextFactory cannot be null");
        }

        /// <summary>
        /// When a problem is submitted, it updates the values in the database accordingly.
        /// If a user is unable to solve a problem that they have solved previously, the <see cref="UserProgress.TotalSolvedProblems"/> will not decrease.
        /// </summary>
        /// <param name="problem">The problem which has been submitted.</param>
        /// <param name="submission">The submission for the problem.</param>
        public async void UpdateProblemData(Problem problem, Submission submission)
        {
            using (var context = await _contextFactory.CreateDbContextAsync())
            {
                var user = await context.UserProgress.FirstOrDefaultAsync(u => u.Id == 1); // To be changes for more users

                if (user != null)
                {
                    if (problem.Status == ProblemStatus.NotTried)
                    {
                        if (submission.Result == SubmissionResult.Success)
                        {
                            user.TotalSolvedProblems++;
                        }
                        else
                        {
                            user.TotalUnsolvedProblems++;
                        }
                    }
                    else if (problem.Status != ProblemStatus.Solved)
                    {
                        if (submission.Result == SubmissionResult.Success)
                        {
                            user.TotalSolvedProblems++;
                            user.TotalUnsolvedProblems--;
                        }
                    }
                }

                if (submission.Result == SubmissionResult.Success)
                    problem.Status = ProblemStatus.Solved;
                else if (problem.Status != ProblemStatus.Solved)
                    problem.Status = ProblemStatus.Unsolved;

                await context.SaveChangesAsync();
            }
        }
    }
}
