using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.Serialization;

namespace CustomDSATrainer.Application
{
    public class UserSourceLinker
    {
        private static string pathToOutputFile = "AIService\\UserOutput.txt";
        public void RunCppExecutable(string pathToExecutable, string pathToInputFile)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = pathToExecutable,
                Arguments = $"",
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = startInfo})
            {
                process.Start();

                List<string> inputList = InputFileParser.ParseInputFile(pathToInputFile);

                foreach (string c in inputList)
                {
                    process.StandardInput.WriteLine(c);
                }

                string output = process.StandardOutput.ReadToEnd();

                process.WaitForExit();

                if (!string.IsNullOrEmpty(output))
                {
                    File.WriteAllText(Path.GetFullPath(pathToOutputFile), output);
                }
            }
        }
    }
}
