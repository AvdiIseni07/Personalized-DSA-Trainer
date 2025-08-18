using CustomDSATrainer.Application;
using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Persistance;
using Microsoft.EntityFrameworkCore;

namespace CustomDSATrainer.Services
{
    public class ProblemService
    {
        private Problem? _currentActiveProblem { get; set; }
        private readonly ProjectDbContext _context;

        public Problem? CurrentActiveProblem
        {
            get => _currentActiveProblem;
            set => _currentActiveProblem = value;
        }

        public ProblemService(ProjectDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context), "Conext cannot be null.");
        }

        public void SubmitProblem(Problem problem, string pathToExe)
        {
            if (problem == null) { throw new ArgumentNullException(nameof(problem), "Problem cannot be null."); }

            Submission submission = new Submission { ProblemId = problem.Id, Id = 0, PathToExecutable = pathToExe };
            submission.RunSumbission(problem.Inputs, problem.Outputs);

            var user = _context.UserProgress.FirstOrDefault(u => u.Id == 1);

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

            _context.SaveChanges();
        }

        public string? AiReview(Problem problem, string pathToSource)
        {
            AIReview currentReview = new AIReview { ProblemId = problem.Id, PathToCPPFile = pathToSource, ProblemStatus = problem.Status };
            string userSource = File.ReadAllText(Path.GetFullPath(pathToSource));

            currentReview.Review = PythonAIService.ReviewProblem(problem.Statement!, userSource, problem.Status == ProblemStatus.Solved);

            //currentReview.SaveToDatabase();

            return currentReview.Review;
        }
    }
}
