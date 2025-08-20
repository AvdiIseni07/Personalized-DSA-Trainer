using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Domain.Interfaces.Repositories;
using CustomDSATrainer.Domain.Interfaces.Services;
using CustomDSATrainer.Domain.Validators;
using CustomDSATrainer.Persistance;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace CustomDSATrainer.Application.Services
{
    public class SubmissionService : ISubmissionService
    {
        private readonly ITestCaseService _testCaseService;
        private readonly ISubmissionRepository _submissionRepository;
        public SubmissionService(ITestCaseService testCaseService, ISubmissionRepository submissionRepository)
        {
            _testCaseService = testCaseService              ?? throw new ArgumentNullException(nameof(testCaseService), "TestCaseService cannot be null.");
            _submissionRepository = submissionRepository    ?? throw new ArgumentNullException(nameof(submissionRepository), "SubmissionRepository cannot be null");
        }

        public void RunSumbission(Submission submission, string _inputs, string _outputs)
        {
            submission.Result = SubmissionResult.Success;

            List<string> inputs = _inputs.Split('!').ToList();
            List<string> outputs = _outputs.Split('!').ToList();

            for (int i = 0; i < 7; i++)
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

        public void SaveToDatabase(Submission submission)
        {
            _submissionRepository.SaveToDatabase(submission);
        }
    }
}
