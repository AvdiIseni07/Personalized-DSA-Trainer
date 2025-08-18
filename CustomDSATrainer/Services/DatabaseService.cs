namespace CustomDSATrainer.Services
{
    public class DatabaseService
    {
        private string _connectionString;

        public DatabaseService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("WebApiDatabase") ?? throw new InvalidOperationException("Database connection string not found");
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }

        public void init(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("WebApiDatabase") ?? throw new InvalidOperationException("Database connection string not found");
        }
    }
}
