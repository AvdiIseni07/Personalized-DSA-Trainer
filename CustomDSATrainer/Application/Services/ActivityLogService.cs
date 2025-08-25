using CustomDSATrainer.Domain.Interfaces;
using CustomDSATrainer.Domain.Interfaces.Services;
using CustomDSATrainer.Persistance;
using CustomDSATrainer.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace CustomDSATrainer.Application.Services
{
    public class ActivityLogService : IActivityLogService
    {
        private readonly ProjectDbContext _context;
        private readonly ICurrentUserProgress _currentUser;
        private readonly ILogger<ActivityLogService> _logger;
        private const int DATE_PREFIX = 10;
        public ActivityLogService(ProjectDbContext context, ICurrentUserProgress currentUser, ILogger<ActivityLogService> logger)
        {
            _context = context          ?? throw new ArgumentNullException(nameof(context), "DbContext cannot be null.");
            _currentUser = currentUser  ?? throw new ArgumentNullException(nameof(currentUser), "CurrentUser cannot be null.");
            _logger = logger            ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");
        }
        /// <summary>
        /// First checks if the current day is already saved into the user's database. If it isn't it then adds it.
        /// Additionally it also checks if the DB contains yesterday's day.
        /// If it does it adds +1 to the logging streak. Otherwise it resets it to 1.
        /// </summary>
        public void LogToday()
        {
            _logger.LogInformation("Logging today.");

                string date = DateTime.Now.ToString();
                date = date.Substring(0, DATE_PREFIX);

                var userFound = _context.UserProgress.FirstOrDefault(u => u.Id == _currentUser.CurrentUserId);
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

                        _context.SaveChanges();
                }
            }
        }
    }
}
