using CustomDSATrainer.Domain.Interfaces;
using CustomDSATrainer.Domain.Interfaces.Services;
using CustomDSATrainer.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CustomDSATrainer.Application.Services
{
    public class ActivityLogService : IActivityLogService
    {
        private readonly IDbContextFactory<ProjectDbContext> _dbContextFactory;
        public ActivityLogService(IDbContextFactory<ProjectDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory), "DbContextFactoryc cannot be null");
        }
        public void LogToday()
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                string date = DateTime.Now.ToString();
                date = date.Substring(0, 10);

                var userFound = context.UserProgress.FirstOrDefault(u => u.Id == 1); // Later to be changed for more users
                if (userFound != null)
                {
                    List<string> daysLogged = userFound.DaysLogged.Split(',').ToList();
                    if (!daysLogged.Contains(date))
                    {
                        if (userFound.DaysLogged == string.Empty)
                            userFound.DaysLogged = date;
                        else
                            userFound.DaysLogged += $",{date}";

                        userFound.TotalDaysLogged++;

                        string yesterday = DateTime.Now.AddDays(-1).ToString();
                        yesterday = yesterday.Substring(0, 10);

                        if (daysLogged.Contains(yesterday))
                        {
                            userFound.LoggingStreak++;
                        }
                        else
                        {
                            userFound.LoggingStreak = 1;
                        }

                        context.SaveChanges();
                    }
                }
            }
        }
    }
}
