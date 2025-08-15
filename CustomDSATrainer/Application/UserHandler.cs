using CustomDSATrainer.Domain;
using CustomDSATrainer.Persistance;
using CustomDSATrainer.Shared;
using Microsoft.EntityFrameworkCore;

namespace CustomDSATrainer.Application
{
    public static class UserHandler
    {
        public static void InitUser()
        {
            // Later to be changed for multiple users
            var optionsBuilder = new DbContextOptionsBuilder<ProjectDbContext>();
            optionsBuilder.UseSqlite(SharedValues.SqliteDatasource);

            using (var context = new ProjectDbContext(optionsBuilder.Options))
            {
                var existingUser = context.UserProgress.FirstOrDefault(u => u.Id == 1);
                if (existingUser == null)
                {
                    UserProgress userProgress = new UserProgress { Id = 1 };
                    context.UserProgress.Add(userProgress);

                    context.SaveChanges();
                }
            }
        }
    }
}
