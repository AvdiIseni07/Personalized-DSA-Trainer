using CustomDSATrainer.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CustomDSATrainer.Persistance.Repositories
{
    /// <summary>
    /// The repository for logging the daily activity of the user.
    /// </summary>
    public class ActivityLogRepository : IActivityLogRepository
    {
        private readonly ProjectDbContext _context;
        public ActivityLogRepository(ProjectDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds the current day to the database.
        /// It also checks whether the day before is also in the database.
        /// If it is it increases the daily streak +1.
        /// Otherwise it resets it to 1.
        /// </summary>
        public async Task LogToday()
        {

            string date = DateTime.Now.ToString();
            date = date.Substring(0, 10);

            var userFound = await _context.UserProgress.FirstOrDefaultAsync(u => u.Id == 1); // Later to be changed for more users
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
                }
            }
        }
    }
}
