namespace CustomDSATrainer.Domain.Interfaces.Repositories
{
    public interface IActivityLogRepository
    {
        Task LogToday();
    }
}
