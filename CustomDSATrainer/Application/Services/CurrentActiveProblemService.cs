using CustomDSATrainer.Domain;

namespace CustomDSATrainer.Application.Services
{
    public class CurrentActiveProblemService
    {
        private Problem? problem;

        public Problem? CurrentProblem
        {
            get => problem;
            set => problem = value;
        }
    }
}
