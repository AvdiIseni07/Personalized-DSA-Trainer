using FluentValidation;

namespace CustomDSATrainer.Domain.Validators
{
    public class ProblemValidator : AbstractValidator<Problem>
    {
        public ProblemValidator()
        {
            RuleFor(problem => problem.Id).NotNull();
            RuleFor(problem => problem.Title).NotEmpty();
            RuleFor(problem => problem.Statement).NotEmpty();
            RuleFor(problem => problem.Categories).NotEmpty();
            RuleFor(problem => problem.Difficulty).NotEmpty();
            RuleFor(problem => problem.Inputs).NotEmpty();
            RuleFor(problem => problem.Outputs).NotEmpty();
            RuleFor(problem => problem.Hint).NotEmpty();
            RuleFor(problem => problem.Status).IsInEnum();
        }
    }
}
