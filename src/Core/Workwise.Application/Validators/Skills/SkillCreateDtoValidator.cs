using FluentValidation;
using Workwise.Application.Dtos;

namespace Workwise.Application.Validators.Skills
{
    public class SkillCreateDtoValidator : AbstractValidator<SkillCreateDto>
    {
        public SkillCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MinimumLength(3).WithMessage("Name must be at least 3 characters long.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.")
                .Matches(@"^[a-zA-Z0-9\s]*$").WithMessage("Name can only contain letters, numbers, and spaces");
        }
    }
}
