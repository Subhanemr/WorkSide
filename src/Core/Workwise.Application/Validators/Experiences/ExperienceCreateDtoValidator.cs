using FluentValidation;
using Workwise.Application.Dtos;

namespace Workwise.Application.Validators.Experiences
{
    public class ExperienceCreateDtoValidator : AbstractValidator<ExperienceCreateDto>
    {
        public ExperienceCreateDtoValidator()
        {
            RuleFor(x => x.About)
                .NotEmpty().WithMessage("About is required")
                .Length(2, 3000).WithMessage("About max characters is 2-3000")
                .Matches(@"^[a-zA-Z0-9\s,\.]*$").WithMessage("About can only contain letters, numbers, spaces, commas, and periods");
        }
    }
}
