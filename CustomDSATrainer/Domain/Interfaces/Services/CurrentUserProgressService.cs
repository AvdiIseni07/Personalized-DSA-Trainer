using CustomDSATrainer.Persistence.Repositories;

namespace CustomDSATrainer.Domain.Interfaces.Services
{
    /// <summary>
    /// Tracks the variables regarding the current active user. Currently there is only one user.
    /// This will be used more if multiple users are ever implement.
    /// </summary>
    public class CurrentUserProgressService : ICurrentUserProgress
    {
        public int CurrentUserId { get; set; } = 1;
    }
}
