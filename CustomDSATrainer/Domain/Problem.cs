using Azure.Core.Pipeline;
using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Collections;
using System.Diagnostics;
using System.Numerics;


namespace CustomDSATrainer.Domain
{
    public class Problem
    {
        public required int Id { get; set; }
        public string? Title { get; set; }
        public string? Statement { get; set; }
        public string? Difficulty { get; set; }
        public string?Categories { get; set; } // "category1,category2"
        public ProblemStatus Status { get; set; } = ProblemStatus.Unsolved;
        public string? Inputs { get; set; } = string.Empty;
        public string? Outputs { get; set; } = string.Empty;
      
        public ICollection<TestCase> TestCases { get; set; } = new List<TestCase>();
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
            optionsBuilder.UseSqlite("Data Source=C:/ProgramData/MainDatabase.db");

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
            Submission submission = new Submission { PathToExecutable = pathToExe, ProblemId = Id };
            submission.RunSumbission();

            if (submission.Result == SubmissionResult.Success)
                this.Status = ProblemStatus.Solved;
            else
                this.Status = ProblemStatus.Unsolved;

            this.SaveToDatabase();
        }
    }
}
