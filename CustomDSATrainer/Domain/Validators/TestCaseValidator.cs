using FluentValidation;

namespace CustomDSATrainer.Domain.Validators
{
    public class TestCaseValidator : AbstractValidator<TestCase>
    {
        public TestCaseValidator()
        {
            RuleFor(testCase => testCase.Id).NotNull();
            RuleFor(testCase => testCase.SubmissionId).NotNull();
            RuleFor(testCase => testCase.Verdict).IsInEnum();
            RuleFor(testCase => testCase.Input).NotNull();
            RuleFor(testCase => testCase.ExpectedOutput).NotNull();
        }
    }
}
