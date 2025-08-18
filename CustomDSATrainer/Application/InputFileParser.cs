using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CustomDSATrainer.Application
{
    public static class InputFileParser
    {
        public static List<string> ParseInputFile(string text)
        {
            List<string> result = text.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).ToList();
           
            return result;
        }
    }
}
