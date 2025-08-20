using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Interfaces.Services;
using CustomDSATrainer.Persistance;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CustomDSATrainer.Application.Services
{
    public class SeedingService : ISeedingService
    {
        private readonly IDbContextFactory<ProjectDbContext> _dbContextFactory;
        private readonly string _kaggleCSVPath;
        public SeedingService(IDbContextFactory<ProjectDbContext> dbContextFactory, IConfiguration configuration)
        {
            _dbContextFactory = dbContextFactory;
            _kaggleCSVPath = configuration["SeedData:KaggleCSVPath"] ?? throw new ArgumentNullException(nameof(configuration), "No kaggle CSV path is found.");
        }
        private string GetCategories(string categoryColumn)
        {
            string categories = "";

            if (categoryColumn.Contains("Tree"))
                categories += "Trees ,";
            if (categoryColumn.Contains("Graph"))
                categories += "Graphs ,";
            if (categoryColumn.Contains("Path"))
                categories += "Graphs/Trees ,";
            if (categoryColumn.Contains("String"))
                categories += "String ,";
            if (categoryColumn.Contains("Combination"))
                categories += "Combinatorics ,";

            if (categories.EndsWith(','))
                categories = categories.Remove(categories.Length - 2, 2);

            return categories;
        }
        public async Task SeedKaggleDataset()
        {
            if (!File.Exists(_kaggleCSVPath))
            {
                throw new Exception("CSV file does not exist");
            }

            using (var context = _dbContextFactory.CreateDbContext())
            {
                if (await context.Problem.AnyAsync())
                    return;
            }

            List<Problem> LeetcodeKaggleDataset = new List<Problem>();

            foreach (string line in File.ReadLines(_kaggleCSVPath))
            {
                string[] content = line.Split(',');
                
                string categoryColumn = content[1];
                string categories = GetCategories(categoryColumn);

                Problem p = new Problem
                {
                    Id = 0,
                    Title = content[1],
                    Statement = "",
                    Difficulty = content[3],
                    Categories = (categories == "") ? "/" : categories
                };
                LeetcodeKaggleDataset.Add(p);
            }

            using (var context = _dbContextFactory.CreateDbContext())
            {
                await context.Problem.AddRangeAsync(LeetcodeKaggleDataset);
                await context.SaveChangesAsync();
            }
        }
    }
}
