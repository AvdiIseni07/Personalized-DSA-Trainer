using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Shared;
using System.Diagnostics;

namespace CustomDSATrainer.Application
{
    public class UserSourceLinker
    {
        private TestCase _testCase;
        public UserSourceLinker(TestCase testCase)
        {
            _testCase = testCase;
        }
        public TestCaseVerdict RunCppExecutable()
        {
            TestCaseVerdict currentVerdict = TestCaseVerdict.Passed;
            var startInfo = new ProcessStartInfo
            {
                FileName = _testCase.PathToExecutable,
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

                List<string> inputList = InputFileParser.ParseInputFile(_testCase.Input);

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

                            if (currentTime - TimeOfStarting >= _testCase.TimeLimit)
                            {
                                currentVerdict = TestCaseVerdict.TimeLimitExceeded;

                                process.Kill(true);
                                break;
                            }

                            if (currentMemoryUsage > _testCase.MemoryLimit)
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
                    SharedValues.UserOutput = output;
                }
            }

            return currentVerdict;
        }
    }
}
