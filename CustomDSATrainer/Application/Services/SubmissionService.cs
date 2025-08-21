using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Domain.Interfaces.Repositories;
using CustomDSATrainer.Domain.Interfaces.Services;
using CustomDSATrainer.Domain.Validators;
using FluentValidation.Results;
namespace CustomDSATrainer.Application.Services
{
    /// <summary>
    /// Provides functionality for running a submission and saving it to the database.
    /// </summary>
    public class SubmissionService : ISubmissionService
    {
        private readonly ITestCaseService _testCaseService;
        private readonly ISubmissionRepository _submissionRepository;
        private const int TEST_CASE_AMOUNT = 7;
        public SubmissionService(ITestCaseService testCaseService, ISubmissionRepository submissionRepository)
        {
            _testCaseService = testCaseService              ?? throw new ArgumentNullException(nameof(testCaseService), "TestCaseService cannot be null.");
            _submissionRepository = submissionRepository    ?? throw new ArgumentNullException(nameof(submissionRepository), "SubmissionRepository cannot be null");
        }
        
        /// <summary>
        /// Runs a submission for a selected <see cref="Problem"/>
        /// It goes through all seven test cases and initializes a <see cref="TestCase"/> object for each one.
        /// </summary>
        /// <param name="submission">The submission that needs to be ran.</param>
        /// <param name="_inputs">The inputs for the test case.</param>
        /// <param name="_outputs">The expected outputs for the test case.</param>
        /// <exception cref="Exception">The validator has deemed that the <see cref="TestCase"/> hasn't been initialized correctly.</exception>
        public void RunSumbission(Submission submission, string _inputs, string _outputs)
        {
            submission.Result = SubmissionResult.Success;

            List<string> inputs = _inputs.Split('!').ToList();
            List<string> outputs = _outputs.Split('!').ToList();

            for (int i = 0; i < TEST_CASE_AMOUNT; i++)
            {
                var testCase = new TestCase
                {
                    Id = 0,
                    SubmissionId = submission.Id,
                    PathToExecutable = submission.PathToExecutable,
                    Input = inputs[i],
                    ExpectedOutput = outputs[i]
                };

                TestCaseValidator validator = new TestCaseValidator();
                ValidationResult result = validator.Validate(testCase);

                if (!result.IsValid)
                {
                    throw new Exception(result.Errors.ToArray().ToString());
                }

                TestCaseVerdict verdict = _testCaseService.InitTestCase(testCase);
                _testCaseService.SaveToDatabase(testCase);

                if (verdict == TestCaseVerdict.TimeLimitExceeded)
                {
                    submission.Result = SubmissionResult.TimeLimitExceeded;
                    break;
                }

                if (verdict == TestCaseVerdict.MemoryLimitExceeded)
                {
                    submission.Result = SubmissionResult.MemoryLimitExceeded;
                    break;
                }

                if (verdict == TestCaseVerdict.IncorrectAnswer)
                {
                    submission.Result = SubmissionResult.Failed;
                    break;
                }
            }
        }

        /// <summary>
        /// Saves a <see cref="Submission"/> to the database.
        /// </summary>
        /// <param name="submission">The <see cref="Submission"/>that needs to be saved.</param>
        public void SaveToDatabase(Submission submission)
        {
            _submissionRepository.SaveToDatabase(submission);
        }
    }
}
