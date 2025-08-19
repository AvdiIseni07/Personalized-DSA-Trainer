using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CustomDSATrainer.Persistance.Repositories
{
    public class UserProgressRepository : IUserProgressRepository
    {
        private readonly IDbContextFactory<ProjectDbContext> _contextFactory;
        public UserProgressRepository (IDbContextFactory<ProjectDbContext> contextFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory), "ContextFactory cannot be null");
        }
        public void UpdateProblemData(Problem problem, Submission submission)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var user = context.UserProgress.FirstOrDefault(u => u.Id == 1);

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

                context.SaveChanges();
            }
        }
    }
}
