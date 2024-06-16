using FluentValidation;
using Workwise.Application.Dtos;

namespace Workwise.Application.Validators.Projects
{
    public class ProjectCreateDtoValidator : AbstractValidator<ProjectCreateDto>
    {
        public ProjectCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MinimumLength(3).WithMessage("Name must be at least 3 characters long.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.")
                .Matches(@"^[a-zA-Z0-9\s,\.]*$").WithMessage("Name can only contain letters, numbers, spaces, commas, and periods");

            RuleFor(x => x.Skill)
                .NotEmpty().WithMessage("Skill is required")
                .MinimumLength(3).WithMessage("Skill must be at least 3 characters long.")
                .MaximumLength(100).WithMessage("Skill must not exceed 100 characters.")
                .Matches(@"^[a-zA-Z0-9\s,\.]*$").WithMessage("Skill can only contain letters, numbers, spaces, commas, and periods");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero");

            RuleFor(x => x.PriceTo)
                .GreaterThan(0).WithMessage("PriceTo must be greater than zero")
                .GreaterThanOrEqualTo(x => x.Price).WithMessage("PriceTo must be greater than or equal to Price");

            RuleFor(x => x.Description)
                .MinimumLength(3).WithMessage("Name must be at least 3 characters long.")
                .MaximumLength(1000).WithMessage("Name must not exceed 1000 characters.")
                .Matches(@"^[a-zA-Z0-9\s,\.]*$").WithMessage("Description can only contain letters, numbers, spaces, commas, and periods")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
}
