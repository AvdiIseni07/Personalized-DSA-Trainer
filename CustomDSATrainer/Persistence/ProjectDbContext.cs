using CustomDSATrainer.Application.Services;
using CustomDSATrainer.Domain;
using Microsoft.EntityFrameworkCore;

namespace CustomDSATrainer.Persistance
{
    public class ProjectDbContext : DbContext
    {
        public DbSet<Problem> Problem { get; set; }
        public DbSet<UserProgress> UserProgress{ get; set; }
        public DbSet<Submission> Submissions{ get; set; }
        public DbSet<TestCase> TestCase{ get; set; }
        public DbSet<AIReview> AIReview {  get; set; }
        public DbSet<Search> Search { get; set; }
        private readonly SeedingService _seedingService;
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options)
        { 
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestCase>()
                .HasOne(tc => tc.Submission)
                .WithMany(s => s.TestCases)
                .HasForeignKey(tc => tc.SubmissionId)
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

    