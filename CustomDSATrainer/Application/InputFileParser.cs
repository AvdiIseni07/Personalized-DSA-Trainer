using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CustomDSATrainer.Application
{
    public static class InputFileParser
    {
        public static List<string> ParseInputFile(string pathToInputFile)
        {
            List<string> result = new List<string>();

            string text = File.ReadAllText(pathToInputFile);
            string[] inputs = text.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string input in inputs)
            {
                result.Add(input);
            }
           
            return result;
        }
    }
}
