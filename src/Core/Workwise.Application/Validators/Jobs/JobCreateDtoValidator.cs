using FluentValidation;
using Workwise.Application.Dtos;

namespace Workwise.Application.Validators.Jobs
{
    public class JobCreateDtoValidator : AbstractValidator<JobCreateDto>
    {
        public JobCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .Matches(@"^[a-zA-Z0-9\s,\.]*$").WithMessage("Name can only contain letters, numbers, spaces, commas, and periods");

            RuleFor(x => x.Skill)
                .NotEmpty().WithMessage("Skill is required")
                .Matches(@"^[a-zA-Z0-9\s,\.]*$").WithMessage("Skill can only contain letters, numbers, spaces, commas, and periods");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero");

            RuleFor(x => x.Time)
                .NotEmpty().WithMessage("Time is required")
                .Matches(@"^[a-zA-Z0-9\s,\.]*$").WithMessage("Time can only contain letters, numbers, spaces, commas, and periods");

            RuleFor(x => x.Description)
                .Matches(@"^[a-zA-Z0-9\s,\.]*$").WithMessage("Description can only contain letters, numbers, spaces, commas, and periods")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
}
