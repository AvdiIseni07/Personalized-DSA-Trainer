using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Persistance;
using CustomDSATrainer.Shared;
using Microsoft.EntityFrameworkCore;

namespace CustomDSATrainer.Domain
{
    public class AIReview
    {
        public int Id { get; set; }
        public required string PathToCPPFile { get; set; }
        public string Review { get; set; } = string.Empty;
        public ProblemStatus ProblemStatus { get; set; }
        public int ProblemId { get; set; }
        public Problem Problem { get; set; }
        public AIReview() { }
        public AIReview(string pathToCPPFile, ProblemStatus problemStatus)
        {
            PathToCPPFile = pathToCPPFile;
            ProblemStatus = problemStatus;
        }
        public void SaveToDatabase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProjectDbContext>();
            optionsBuilder.UseSqlite(SharedValues.SqliteDatasource);

            using (var context = new ProjectDbContext(optionsBuilder.Options))
            {
                var existingReview = context.AIReview.Find(Id);

                if (existingReview == null)
                {
                    context.AIReview.Add(this);
                }
                else
                {
                    context.Entry(existingReview).CurrentValues.SetValues(this);
                }

                context.SaveChanges();
            }
        }
    }
}
