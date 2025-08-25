using Microsoft.EntityFrameworkCore;

namespace CustomDSATrainer.Domain.Interfaces.Services
{
    public interface ISeedingService
    {
        Task SeedKaggleDataset();
    }
}
