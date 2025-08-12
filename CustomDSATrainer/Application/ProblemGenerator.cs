using CustomDSATrainer.Domain;
using System.Xml;

namespace CustomDSATrainer.Application
{
    public static class ProblemGenerator
    {   
        public static string categories { get; set; }
        public static string difficulty { get; set; } = "Med.";
        public static string pathToStatement = "AIService/Task/Statement.txt";
        private static string pathToInput = "AIService/Task/Inputs";
        private static string pathToOutput = "AIService/Task/Outputs";
        public static Problem GenerateFromPrompt()
        {
            PythonAIService.GenerateProblemFromPrompt(categories);

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

            for (int i = 1; i <= 7; i ++)
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
    }
}
