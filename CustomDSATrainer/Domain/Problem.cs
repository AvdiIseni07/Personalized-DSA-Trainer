using Azure.Core.Pipeline;
using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Persistance;
using Microsoft.EntityFrameworkCore;
using CustomDSATrainer.Shared;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Localization;
using CustomDSATrainer.Application;


namespace CustomDSATrainer.Domain
{
    public class Problem
    {
        public required int Id { get; set; }
        public string? Title { get; set; }
        public string? Statement { get; set; }
        public string? Difficulty { get; set; }
        public string? Categories { get; set; } // "category1,category2"
        public string? Hint { get; set; }
        public ProblemStatus Status { get; set; } = ProblemStatus.NotTried;
        public string? Inputs { get; set; } = string.Empty;
        public string? Outputs { get; set; } = string.Empty;
      
        public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
        public ICollection<AIReview> AIReviews { get; set; } = new List<AIReview>();
        public Problem()
        { }
        public Problem(int id, string title, string statement, string difficulty, string categories)
        {
            Id = id;
            Title = title;
            Statement = statement;
            Difficulty = difficulty;
            Categories = categories;
        }

        public void SaveToDatabase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProjectDbContext>();
            optionsBuilder.UseSqlite(SharedValues.SqliteDatasource);

            using (var context = new ProjectDbContext(optionsBuilder.Options))
            {
                var existingProblem = context.Problem.Find(Id);

                if (existingProblem == null)
                {
                    context.Problem.Add(this);
                }
                else
                {
                    context.Entry(existingProblem).CurrentValues.SetValues(this);
                }

                context.SaveChanges();
            }
        }

        public void Submit(string pathToExe)
        {
            Submission submission = new Submission { ProblemId = this.Id, Id = 0, PathToExecutable = pathToExe};
            submission.SaveToDatabase();
            submission.RunSumbission();
            submission.SaveToDatabase();

            var optionsBuilder = new DbContextOptionsBuilder<ProjectDbContext>();
            optionsBuilder.UseSqlite(SharedValues.SqliteDatasource);

            using (var context = new ProjectDbContext(optionsBuilder.Options))
            {
                var user = context.UserProgress.FirstOrDefault(u => u.Id == 1);

                if (user != null)
                {
                    if (this.Status == ProblemStatus.NotTried)
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
                    else if (this.Status != ProblemStatus.Solved)
                    {
                        if (submission.Result == SubmissionResult.Success)
                        {
                            user.TotalSolvedProblems++;
                            user.TotalUnsolvedProblems--;
                        }
                    }
                }

                context.SaveChanges();
            }

            if (submission.Result == SubmissionResult.Success)
                this.Status = ProblemStatus.Solved;
            else if (this.Status != ProblemStatus.Solved)
                this.Status = ProblemStatus.Unsolved;

            this.SaveToDatabase();
        }
        private string pathToAIReviewResult = "AIService/CodeReview/Result.txt";
        public string? AiReview(string pathToSource)
        {
            AIReview currentReview = new AIReview {ProblemId = this.Id, PathToCPPFile = pathToSource, ProblemStatus = this.Status };

            if (this.Status != ProblemStatus.Solved)
                CodeReviewer.ReviewUnsolvedProblem(pathToSource);
            else
                CodeReviewer.ReviewSolvedProblem(pathToSource);

            currentReview.Review = File.ReadAllText(Path.GetFullPath(pathToAIReviewResult));

            currentReview.SaveToDatabase();

            return currentReview.Review;
        }
    }
}
