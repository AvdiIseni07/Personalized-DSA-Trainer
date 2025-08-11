using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Persistance;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Numerics;


namespace CustomDSATrainer.Domain
{
    public class Problem
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required string Statement { get; set; }
        public required string Difficulty { get; set; }
        public required string Categories { get; set; } // "category1,category2"
        public ProblemStatus Status { get; set; } = ProblemStatus.Unsolved;
        
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
                context.Problem.Add(this);
                context.SaveChanges();
            }
        }
    }
}
