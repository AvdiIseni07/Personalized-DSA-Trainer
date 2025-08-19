using CustomDSATrainer.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CustomDSATrainer.Persistance.Repositories
{
    public class ActivityLogRepository : IActivityLogRepository 
    {
        private readonly IDbContextFactory<ProjectDbContext> _dbContextFactory;
        public ActivityLogRepository(IDbContextFactory<ProjectDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
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
