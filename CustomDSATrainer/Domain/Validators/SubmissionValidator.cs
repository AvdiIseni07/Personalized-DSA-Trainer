using FluentValidation;

namespace CustomDSATrainer.Domain.Validators
{
    public class SubmissionValidator : AbstractValidator<Submission>
    {
        public SubmissionValidator()
        {
            RuleFor(submission => submission.Id).NotNull();
            RuleFor(submission => submission.PathToExecutable).NotNull();
            RuleFor(submission => submission.ProblemId).NotNull();
            RuleFor(submission => submission.Result).IsInEnum();
        }
    }
}
