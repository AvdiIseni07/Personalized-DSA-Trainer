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
        private static Random random = new Random();

        private static Problem InitProblem(List<string> problemData)
        {
            string statement = problemData[0];
            string title = statement.Split('\n')[0];
            string inputs = problemData[1];
            string outputs = problemData[2];
            string hint = problemData[3];
            string categories = problemData[4];

            title = title.Remove(0, 7);
            statement = statement.Remove(0, 7);

            Problem problem = new Problem
            {
                Id = 0,
                Title = title,
                Statement = statement,
                Difficulty = difficulty,
                Categories = categories,
                Hint = hint,
                Inputs = inputs,
                Outputs = outputs
            };

            problem.SaveToDatabase();

            return problem;
        }
        public static Problem GenerateFromPrompt(string categories)
        {
            List<string> problemData = PythonAIService.GenerateProblemFromPrompt(categories);

            return InitProblem(problemData);
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

                    List<string> problemData = PythonAIService.GenerateProblemFromUnsolved(prompt);
                    generatedProblem = InitProblem(problemData);
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
