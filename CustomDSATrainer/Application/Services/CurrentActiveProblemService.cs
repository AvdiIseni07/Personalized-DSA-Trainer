using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Interfaces.Services;

namespace CustomDSATrainer.Application.Services
{
    /// <summary>
    /// Provides functionality to set and get the current active problem for the user.
    /// </summary
    public class CurrentActiveProblemService : ICurrentActiveProblemService
    {
        private Problem? problem;

        public Problem? CurrentProblem
        {
            get => problem;
            set => problem = value;
        }
    }
}
