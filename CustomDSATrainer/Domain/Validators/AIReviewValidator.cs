using FluentValidation;

namespace CustomDSATrainer.Domain.Validators
{
    public class AIReviewValidator : AbstractValidator<AIReview>
    {
        public AIReviewValidator()
        {
            RuleFor(aiReview => aiReview.Id).NotEmpty();
            RuleFor(aiReview => aiReview.PathToCPPFile).NotEmpty();
            RuleFor(aiReview => aiReview.ProblemId).NotEmpty();
            RuleFor(aiReview => aiReview.ProblemStatus).IsInEnum();
        }
    }
}
