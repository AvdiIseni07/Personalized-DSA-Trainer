namespace CustomDSATrainer.Domain.Interfaces.Services
{
    public interface ICurrentActiveProblemService
    {
        Problem? CurrentProblem { get; set; }
    }
}
