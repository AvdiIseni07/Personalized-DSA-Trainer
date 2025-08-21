using FluentValidation;

namespace CustomDSATrainer.Domain.Validators
{
    public class SubmissionValidator : AbstractValidator<Submission>
    {
        public SubmissionValidator()
        {
            RuleFor(submission => submission.Id).NotEmpty();
            RuleFor(submission => submission.PathToExecutable).NotEmpty();
            RuleFor(submission => submission.ProblemId).NotEmpty();
            RuleFor(submission => submission.Result).IsInEnum();
        }
    }
}
