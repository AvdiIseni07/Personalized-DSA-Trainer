using CustomDSATrainer.Application;
using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Domain.Interfaces.Repositories;
using CustomDSATrainer.Domain.Interfaces.Services;
using CustomDSATrainer.Persistance;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace CustomDSATrainer.Application.Services
{
    public class TestCaseService : ITestCaseService
    {
        private IUserOutputService _userOutputService;
        private readonly IUserSourceLinkerService _userSourceLinkerService;
        private readonly ITestCaseRepository _testCaseRepository;
        public TestCaseService(IUserOutputService userOutputService, IUserSourceLinkerService userSourceLinkerService, ITestCaseRepository testCaseRepository)
        {
            _userOutputService = userOutputService              ?? throw new ArgumentNullException(nameof(userOutputService), "UserOutputService cannot be null.");
            _userSourceLinkerService = userSourceLinkerService  ?? throw new ArgumentNullException(nameof(userSourceLinkerService), "UserSourceLinkerService cannot be null.");
            _testCaseRepository = testCaseRepository            ?? throw new ArgumentNullException(nameof(testCaseRepository), "TestCaseRepository cannot be null.");
        }

        public TestCaseVerdict InitTestCase(TestCase testCase)
        {
            if (testCase == null) throw new ArgumentNullException(nameof(testCase), "Test case cannot be null.");
            testCase.Verdict = _userSourceLinkerService.RunCppExecutable(testCase);

            if (testCase.Verdict == TestCaseVerdict.Passed)
                testCase.Verdict = RunTest(testCase.ExpectedOutput);

            return testCase.Verdict;
        }

        private TestCaseVerdict RunTest(string _expectedOutput)
        {
            TestCaseVerdict verdict = TestCaseVerdict.Passed;

            string[] userOutput = _userOutputService.UserOutput.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            string[] expectedOutput = _expectedOutput.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            if (userOutput.Length < expectedOutput.Length)
            {
                verdict = TestCaseVerdict.TooFewArguments;
                return verdict;
            }

            if (userOutput.Length > expectedOutput.Length)
            {
                verdict = TestCaseVerdict.TooManyArguments;
                return verdict;
            }

            for (int i = 0; i < userOutput.Length; i++)
            {
                if (userOutput[i] != expectedOutput[i])
                {
                    verdict = TestCaseVerdict.IncorrectAnswer;
                    break;
                }
            }

            return verdict;
        }

        public void SaveToDatabase(TestCase testCase)
        {
            _testCaseRepository.SaveToDatabase(testCase);
        }
    }
}

