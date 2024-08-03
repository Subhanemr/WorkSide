using FluentValidation;
using Workwise.Application.Dtos;

namespace Workwise.Application.Validators.Comment
{
    public class AddJobCommentDtoValidator : AbstractValidator<AddJobCommentDto>
    {
        public AddJobCommentDtoValidator()
        {
            RuleFor(x => x.Comment)
                .NotEmpty().WithMessage("Content cannot be empty.")
                .Length(1, 1000).WithMessage("Content must be between 1 and 1000 characters.")
                .Matches(@"^[a-zA-Z0-9\s,.!?-]+$").WithMessage("Content contains invalid characters.");
        }
    }
}
