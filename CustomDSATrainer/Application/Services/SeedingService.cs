using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Interfaces.Services;
using CustomDSATrainer.Persistance;
using Microsoft.EntityFrameworkCore;

namespace CustomDSATrainer.Application.Services
{
    /// <summary>
    /// Provides functionality to seed the kaggle dataset.
    /// It gets the CSV file based on the configuration and parses it.
    /// </summary>
    public class SeedingService : ISeedingService
    {
        private readonly IDbContextFactory<ProjectDbContext> _dbContextFactory;
        private readonly string _kaggleCSVPath;
        public SeedingService(IDbContextFactory<ProjectDbContext> dbContextFactory, IConfiguration configuration)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory), "DbContextFactory cannot be null");
            _kaggleCSVPath = configuration["SeedData:KaggleCSVPath"] ?? throw new ArgumentNullException(nameof(configuration), "No kaggle CSV path is found.");
        }
        /// <summary>
        /// Returns the categories it is able to identify from the title.
        /// </summary>
        /// <param name="categoryColumn">The title of the problem.</param>
        /// <returns>A List of the categories it was able to identify.</returns>
        private string GetCategories(string categoryColumn)
        {
            List<string> categories = new List<string>();

            if (categoryColumn.Contains("Tree"))
                categories.Add("Trees");
            if (categoryColumn.Contains("Graph"))
                categories.Add("Graphs");
            if (categoryColumn.Contains("Path"))
                categories.Add("Graphs/Trees");
            if (categoryColumn.Contains("String"))
                categories.Add("String");
            if (categoryColumn.Contains("Combination"))
                categories.Add("Combinatorics");

            return string.Join(',', categories);
        }
        /// <summary>
        /// Seeds the kaggle dataset into the database.
        /// </summary>
        /// <exception cref="Exception">The CSV file hasn't been found in the specified path.</exception>
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
