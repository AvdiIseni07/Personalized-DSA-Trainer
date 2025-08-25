using FluentValidation;

namespace CustomDSATrainer.Domain.Validators
{
    public class TestCaseValidator : AbstractValidator<TestCase>
    {
        public TestCaseValidator()
        {
            RuleFor(testCase => testCase.Id).NotEmpty();
            RuleFor(testCase => testCase.SubmissionId).NotEmpty();
            RuleFor(testCase => testCase.Verdict).IsInEnum();
            RuleFor(testCase => testCase.Input).NotEmpty();
            RuleFor(testCase => testCase.ExpectedOutput).NotEmpty();
        }
    }
}
