using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Persistance;
using CustomDSATrainer.Shared;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace CustomDSATrainer.Application
{
    public static class ProblemGenerator
    {   
        public static string difficulty { get; set; } = "Med.";
        public static string pathToStatement = "AIService/Task/Statement.txt";
        private static string pathToInput = "AIService/Task/Inputs";
        private static string pathToOutput = "AIService/Task/Outputs";
        private static Random random = new Random();

        private static Problem InitProblem(string categories)
        {
            string statement = File.ReadAllText(Path.GetFullPath(pathToStatement));
            string title = File.ReadAllLines(Path.GetFullPath(pathToStatement))[0];

            title = title.Remove(0, 7);
            statement = statement.Remove(0, 7);

            Problem problem = new Problem
            {
                Id = 0,
                Title = title,
                Statement = statement,
                Difficulty = difficulty,
                Categories = categories
            };

            for (int i = 1; i <= 7; i++)
            {
                var inputFile = Directory.GetFiles(pathToInput, i.ToString(), SearchOption.AllDirectories)[0];
                var outputFile = Directory.GetFiles(pathToOutput, i.ToString(), SearchOption.AllDirectories)[0];

                if (problem.Inputs != string.Empty)
                    problem.Inputs += "!\n";
                problem.Inputs += File.ReadAllText(inputFile);

                if (problem.Outputs != string.Empty)
                    problem.Outputs += "!\n";

                problem.Outputs += File.ReadAllText(outputFile);
            }

            problem.SaveToDatabase();

            return problem;
        }
        public static Problem GenerateFromPrompt(string categories)
        {
            PythonAIService.GenerateProblemFromPrompt(categories);

            return InitProblem(categories);
        }

        public static Problem? GenerateProblemFromUnsolved()
        {
            Problem? generatedProblem = null;

            var optionsBuilder = new DbContextOptionsBuilder<ProjectDbContext>();
            optionsBuilder.UseSqlite(SharedValues.SqliteDatasource);

            using (var context = new ProjectDbContext(optionsBuilder.Options))
            {
                var results = context.Problem.Where(p => p.Status == ProblemStatus.Unsolved).ToList();
                List<string> categories = new List<string>();

                foreach (var problem in results)
                {
                    if (problem.Categories == null)
                        continue;

                    string[] currentCategories = problem.Categories.Split(',');
                    foreach(string category in currentCategories)
                    {
                        if (!categories.Contains(category))
                            categories.Add(category);
                    }
                }

                if (categories.Count > 0)
                {
                    string prompt = string.Empty;
                    foreach (string category in categories)
                    {
                        if (prompt == string.Empty)
                            prompt = category;
                        else
                            prompt += ", " + category;
                    }

                    PythonAIService.GenerateProblemFromUnsolved(prompt);
                    generatedProblem = InitProblem(prompt);
                }
            }

            return generatedProblem;
        }

        public static Problem? Revision()
        {
            Problem? generatedProblem = null;

            var optionsBuilder = new DbContextOptionsBuilder<ProjectDbContext>();
            optionsBuilder.UseSqlite(SharedValues.SqliteDatasource);

            using (var context = new ProjectDbContext(optionsBuilder.Options))
            {
                var results = context.Problem.Where(p => p.Status == ProblemStatus.Solved).ToList();

                if (results.Count > 0)
                {
                    int chosenProblem = random.Next(0, results.Count);
                    generatedProblem = results[chosenProblem];
                }
            }

            return generatedProblem;
        }
 
        public static Problem? RevisionWithCategories(string categories)
        {
            Problem? generatedProblem = null;

            var optionsBuilder = new DbContextOptionsBuilder<ProjectDbContext>();
            optionsBuilder.UseSqlite(SharedValues.SqliteDatasource);

            using (var context = new ProjectDbContext(optionsBuilder.Options))
            {
                var results = context.Problem.Where(p => p.Categories.Contains(categories)).ToList();

                if (results.Count > 0)
                {
                    int chosenProblem = random.Next(0, results.Count);
                    generatedProblem = results[chosenProblem];
                }
            }

            return generatedProblem;
        }
    }
}
