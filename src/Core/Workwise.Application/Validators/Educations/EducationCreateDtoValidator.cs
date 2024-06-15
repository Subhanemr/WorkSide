using FluentValidation;
using Workwise.Application.Dtos;

namespace Workwise.Application.Validators.Educations
{
    public class EducationCreateDtoValidator : AbstractValidator<EducationCreateDto>
    {
        public EducationCreateDtoValidator()
        {
            RuleFor(x => x.School_University)
                .Length(2, 1000).WithMessage("School or University max characters is 2-1000")
                .NotEmpty().WithMessage("School/University is required.");

            RuleFor(x => x.From)
                .NotEmpty().WithMessage("From date is required.");

            RuleFor(x => x.To)
                .NotEmpty().WithMessage("To date is required.")
                .GreaterThanOrEqualTo(x => x.From).WithMessage("To date must be after or equal to From date.");

            RuleFor(x => x.Degree)
                .Length(2, 1000).WithMessage("Degree max characters is 2-1000")
                .NotEmpty().WithMessage("Degree is required.");
        }
    }
}
