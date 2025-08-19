namespace CustomDSATrainer.Domain.Interfaces.Services
{
    public interface IDatabaseService
    {
        string GetConnectionString();
        void init(IConfiguration configuration);
    }
}
