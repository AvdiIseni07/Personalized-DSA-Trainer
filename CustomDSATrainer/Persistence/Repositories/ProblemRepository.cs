using CustomDSATrainer.Application.Services;
using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;


namespace CustomDSATrainer.Persistance.Repositories
{
    /// <summary>
    /// Repository for <see cref="Problem"/>
    /// </summary>
    public class ProblemRepository : IProblemRepository
    {
        private readonly ProjectDbContext _context;
        private Random random = new Random();

        public ProblemRepository(ProjectDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context), "DbContext cannot be null.");
        }

        /// <summary>
        /// Tries to get a problem from the database from the given ID.
        /// </summary>
        /// <param name="id">The ID of the required problem.</param>
        /// <returns>Returns the required <see cref="Problem"/>, if found in the database.</returns>
        public async Task<Problem?> GetFromId(int id)
        {
            Problem? problem = null;

            problem = await _context.Problem.FirstOrDefaultAsync(p => p.Id == id);

            return problem;
        }

        public async Task<List<int>> GetFromSearch(string query)
        {
            List<int> results = await _context.Problem.Where(p => p.Title.Contains(query) || p.Statement.Contains(query))
                .OrderBy(p => p.Difficulty == "Easy" ? 1 : p.Difficulty == "Medium" ? 2 : p.Difficulty == "Hard" ? 3 : 4)
                .Select(p => p.Id).ToListAsync();

            return results;
        }

        /// <summary>
        /// Saves a <see cref="Problem"/> to the database.
        /// If it already exists, it only updates the values.
        /// </summary>
        /// <param name="problem">The problem that needs to be saved.</param>
        public async void SaveToDatabase(Problem problem)
        {
            var existingProblem = await _context.Problem.FindAsync(problem.Id);

            if (existingProblem == null)
            {
                _context.Problem.Add(problem);
            }
            else
            {
                _context.Entry(existingProblem).CurrentValues.SetValues(problem);
            }

            await _context.SaveChangesAsync();

        }

        /// <summary>
        /// Gets all the categories of the unsolved problems.
        /// Also gets a random difficulty from the unsolved problems.
        /// It is used for <see cref="ProblemGeneratorService.GenerateProblemFromUnsolved"/>.
        /// </summary>
        /// <returns></returns>
        public async Task<Tuple<string, string>> GetUnsolvedData()
        {
            string prompt = string.Empty, difficulty = string.Empty;
            var results = await _context.Problem.Where(p => p.Status == ProblemStatus.Unsolved).ToListAsync();
            List<string> categories = new List<string>();
            List<string> difficulties = new List<string>();

            foreach (var problem in results)
            {
                if (problem.Categories == null || problem.Difficulty == null)
                    continue;

                string[] currentCategories = problem.Categories.Split(',');
                foreach (string category in currentCategories)
                {
                    if (!categories.Contains(category))
                        categories.Add(category);
                }

                if (!difficulties.Contains(problem.Difficulty))
                    difficulties.Add(problem.Difficulty);
            }

            if (categories.Count > 0)
            {
                foreach (string category in categories)
                {
                    if (prompt == string.Empty)
                        prompt = category;
                    else
                        prompt += ", " + category;
                }

                int difficultyIndex = random.Next(0, difficulties.Count);
                difficulty = difficulties[difficultyIndex];

            }

            return new Tuple<string, string>(prompt, difficulty);
        }

        /// <summary>
        /// Selects a random <see cref="Problem"/> that has been solved so the user can try to solve it again.
        /// It is used by <see cref="ProblemGeneratorService.Revision"/>.
        /// </summary>
        /// <returns>The selected <see cref="Problem"/>, if the process was successful.</returns>
        public async Task<Problem?> GetRevision()
        {
            Problem? problem = null;

            var results = await _context.Problem.Where(p => p.Status == ProblemStatus.Solved).ToListAsync();

            if (results.Count > 0)
            {
                int chosenProblem = random.Next(0, results.Count);
                problem = results[chosenProblem];
            }

            return problem;
        }

        /// <summary>
        /// Selects a random <see cref="Problem"/> that has the specified categories and has been solved so that the user can try to solve it again.
        /// It is used by <see cref="ProblemGeneratorService.RevisionWithCategories(string)"/>.
        /// </summary>
        /// <param name="categories">The categories that the problem must have.</param>
        /// <returns>Returns the selected <see cref="Problem"/>, if the process was successful.</returns>
        public async Task<Problem?> GetRevisionWithCategories(string categories)
        {
            Problem? problem = null;

            var results = await _context.Problem.Where(p => p.Categories.Contains(categories)).ToListAsync();

            if (results.Count > 0)
            {
                int chosenProblem = random.Next(0, results.Count);
                problem = results[chosenProblem];
            }

            return problem;
        }
    }
}
