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
        private const int DATE_PREFIX = 10;
        public ActivityLogService(IDbContextFactory<ProjectDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory), "DbContextFactory cannot be null");
        }
        /// <summary>
        /// First checks if the current day is already saved into the user's database. If it isn't it then adds it.
        /// Additionally it also checks if the DB contains yesterday's day.
        /// If it does it adds +1 to the logging streak. Otherwise it resets it to 1.
        /// </summary>
        public void LogToday()
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                string date = DateTime.Now.ToString();
                date = date.Substring(0, DATE_PREFIX);

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
