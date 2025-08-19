using CustomDSATrainer.Application;
using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Domain.Interfaces.Services;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace CustomDSATrainer.Application.Services
{
    public class UserSourceLinkerService : IUserSourceLinkerService
    {
        private UserOutputService _userOutputService;
        public UserSourceLinkerService(UserOutputService userOutputService)
        {
            _userOutputService = userOutputService;
        }
        public TestCaseVerdict RunCppExecutable(TestCase testCase)
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
                
                string output = process.StandardOutput.ReadToEnd();

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
