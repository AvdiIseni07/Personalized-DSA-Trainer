using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Interfaces.Services;

namespace CustomDSATrainer.Application.Services
{
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
