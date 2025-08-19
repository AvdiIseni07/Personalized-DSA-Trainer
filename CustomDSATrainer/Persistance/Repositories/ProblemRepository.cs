using Azure.Core.Pipeline;
using CustomDSATrainer.Application.Services;
using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System;

namespace CustomDSATrainer.Persistance.Repositories
{
    public class ProblemRespository : IProblemRepository
    {
        private readonly IDbContextFactory<ProjectDbContext> _contextFactory;
        private Random random = new Random();

        public ProblemRespository(IDbContextFactory<ProjectDbContext> contextFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory), "ContextFactory cannot be null.");
        }

        public Problem? GetFromId(int id)
        {
            Problem? problem = null;

            using (var context = _contextFactory.CreateDbContext())
            {
                problem = context.Problem.FirstOrDefault(p => p.Id == id);
            }

            return problem;
        }

        public void SaveToDatabase(Problem problem)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var existingProblem = context.Problem.Find(problem.Id);

                if (existingProblem == null)
                {
                    context.Problem.Add(problem);
                }
                else
                {
                    context.Entry(existingProblem).CurrentValues.SetValues(problem);
                }

                context.SaveChanges();
            }
        }

        public Tuple<string, string> GetUnsolvedData()
        {
            string prompt = string.Empty, difficulty = string.Empty;
            using (var context = _contextFactory.CreateDbContext())
            {
                var results = context.Problem.Where(p => p.Status == ProblemStatus.Unsolved).ToList();
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
            }

            return new Tuple<string, string>(prompt, difficulty);
        }
        public Problem? GetRevision()
        {
            Problem? problem = null;

            using (var context = _contextFactory.CreateDbContext())
            {
                var results = context.Problem.Where(p => p.Status == ProblemStatus.Solved).ToList();

                if (results.Count > 0)
                {
                    int chosenProblem = random.Next(0, results.Count);
                    problem = results[chosenProblem];
                }
            }

            return problem;
        }
        public Problem? GetRevisionWithCategories(string categories)
        {
            Problem? problem = null;

            using (var context = _contextFactory.CreateDbContext())
            {
                var results = context.Problem.Where(p => p.Categories.Contains(categories)).ToList();

                if (results.Count > 0)
                {
                    int chosenProblem = random.Next(0, results.Count);
                    problem = results[chosenProblem];
                }
            }

            return problem;
        }
    }
}
