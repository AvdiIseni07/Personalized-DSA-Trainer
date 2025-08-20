using CustomDSATrainer.Domain;
using CustomDSATrainer.Persistance;
using CustomDSATrainer.Application.Services;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace CustomDSATrainer.Application
{
   /* public static class UserHandler
    {
        private static DatabaseService _databaseService;
        public UserHandler(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        public static void InitUser()
        {
            // Later to be changed for multiple users
            var optionsBuilder = new DbContextOptionsBuilder<ProjectDbContext>();
            optionsBuilder.UseSqlite();

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
    }*/
}
