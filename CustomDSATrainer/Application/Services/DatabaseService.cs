using CustomDSATrainer.Domain.Interfaces.Services;

namespace CustomDSATrainer.Application.Services
{
    /// <summary>
    /// Gets the sqlite connection string.
    /// </summary>
    public class DatabaseService : IDatabaseService
    {
        private string _connectionString;

        public DatabaseService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("WebApiDatabase") ?? throw new InvalidOperationException("Database connection string not found.");
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }

        public void init(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("WebApiDatabase") ?? throw new InvalidOperationException("Database connection string not found.");
        }
    }
}
