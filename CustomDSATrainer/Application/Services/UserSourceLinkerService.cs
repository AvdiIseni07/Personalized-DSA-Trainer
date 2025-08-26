using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Domain.Interfaces.Services;
using System.Diagnostics;

namespace CustomDSATrainer.Application.Services
{
    /// <summary>
    /// Provides functionality to run the user's executable.
    /// It routes the I/O through a custom stream. It gives the necessary input and captures the output.
    /// It checks for time and memory limit violations. It doesn't compare the outputs.
    /// </summary>
    public class UserSourceLinkerService : IUserSourceLinkerService
    {
        private IUserOutputService _userOutputService;
        public UserSourceLinkerService(IUserOutputService userOutputService)
        {
            _userOutputService = userOutputService ?? throw new ArgumentNullException(nameof(userOutputService), "UserOutputService cannot be null.");
        }

        /// <summary>
        /// It runs a test case by executing the user-generated exeuctable.
        /// It routes the I/O through a custom stream from where it gives the input (<see cref="TestCase.Input"/>) and captures the output
        /// It also continuously checks for time and memory limit violations.
        /// </summary>
        /// <param name="testCase">The <see cref="TestCase"/> that needs to be ran.</param>
        /// <returns>A <see cref="TestCaseVerdict"/> that tells whether the code exceeded the time or memory limits.</returns>
        public async Task<TestCaseVerdict> RunCppExecutable(TestCase testCase)
        {
            TestCaseVerdict currentVerdict = TestCaseVerdict.Passed;
            var startInfo = new ProcessStartInfo
            {
                FileName = testCase.PathToExecutable,
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
                long TimeOfStarting = ((DateTimeOffset)DateTime.Now).ToUnixTimeMilliseconds();

                List<string> inputList = testCase.Input.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                foreach (string input in inputList)
                {
                    process.StandardInput.WriteLine(input);
                }

                process.StandardInput.Close();

                var monitorThread = new Thread(() => 
                {
                    try
                    {
                        while (!process.HasExited)
                        {
                            long currentTime = ((DateTimeOffset)DateTime.Now).ToUnixTimeMilliseconds();
                            long currentMemoryUsage = process.WorkingSet64 / (1024 * 1024);

                            if (currentTime - TimeOfStarting >= testCase.TimeLimit)
                            {
                                currentVerdict = TestCaseVerdict.TimeLimitExceeded;

                                process.Kill(true);
                                break;
                            }

                            if (currentMemoryUsage > testCase.MemoryLimit)
                            {
                                currentVerdict = TestCaseVerdict.MemoryLimitExceeded;

                                process.Kill(true);
                                break;
                            }

                            Thread.Sleep(50);
                        }
                    }
                    catch { }
                });
                monitorThread.Start();
                
                string output = await process.StandardOutput.ReadToEndAsync();

                process.WaitForExit();

                if (!string.IsNullOrEmpty(output))
                {
                    _userOutputService.UserOutput = output;
                }
            }

            return currentVerdict;
        }
    }
}
