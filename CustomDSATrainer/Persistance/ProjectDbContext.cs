using CustomDSATrainer.Domain;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CustomDSATrainer.Persistance
{
    public class ProjectDbContext : DbContext
    {

        public DbSet<Problem> Problem { get; set; }
        public DbSet<UserProgress> UserProgress{ get; set; }
        public DbSet<Submission> Submissions{ get; set; }
        public DbSet<TestCase> TestCase{ get; set; }
        public DbSet<AIReview> AIReview {  get; set; }
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options)
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            int indx = 0;
            List<Problem> LeetcodeKaggleDataset = new List<Problem>();

            foreach (string line in File.ReadLines("C:/ProgramData/csv_parser/csvtotext.txt"))
            {
                if (indx == 0)
                {
                    indx++;
                    continue;
                }

                string[] content = line.Split(',');
                string categories = "";

                if (content[1].Contains("Tree"))
                    categories += "Trees ,";
                if (content[1].Contains("Graph"))
                    categories += "Graphs ,";
                if (content[1].Contains("Path"))
                    categories += "Graphs/Trees ,";
                if (content[1].Contains("String"))
                    categories += "String ,";
                if (content[1].Contains("Combination"))
                    categories += "Combinatorics ,";

                if (categories.EndsWith(','))
                    categories = categories.Remove(categories.Length - 2, 2);

                Problem p = new Problem
                {
                    Id = indx,
                    Title = content[1],
                    Statement = "",
                    Difficulty = content[3],
                    Categories = (categories == "") ? "/" : categories
                };
                LeetcodeKaggleDataset.Add(p);
                indx++;
            }

            modelBuilder.Entity<Problem>().HasData(LeetcodeKaggleDataset.ToArray());

            modelBuilder.Entity<TestCase>()
                .HasOne(tc => tc.Problem)
                .WithMany(p => p.TestCases)
                .HasForeignKey(tc => tc.ProblemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Submission>()
                .HasOne(s => s.Problem)
                .WithMany(p => p.Submissions)
                .HasForeignKey(s => s.ProblemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AIReview>()
                .HasOne(air => air.Problem)
                .WithMany(p => p.AIReviews)
                .HasForeignKey(air => air.ProblemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Problem>().HasIndex(p => p.Status)
                .HasDatabaseName("idx_Problem_Status");
        }
    }
}

    