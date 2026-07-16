using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Interfaces.Services;
using CustomDSATrainer.Persistance;
using CustomDSATrainer.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CustomDSATrainer.Application.Services
{
    public class UserHandlerService : IUserHandlerService
    {
        private readonly ProjectDbContext _context;
        private readonly ICurrentUserProgress _currentUser;
        public UserHandlerService(ProjectDbContext context, ICurrentUserProgress currentUser)
        {
            _context = context;
            _currentUser = currentUser;
            InitUser();
        }
        private void InitUser()
        {
            int existingUsers = _context.UserProgress.Count();
            if (existingUsers == 0)
            {
                UserProgress userProgress = new UserProgress { Id = 0 };
                _context.UserProgress.Add(userProgress);

                _context.SaveChanges();
            }
        }
    }
}
