using FluentValidation;
using Workwise.Application.Dtos;

namespace Workwise.Application.Validators.Educations
{
    public class EducationCreateDtoValidator : AbstractValidator<EducationCreateDto>
    {
        public EducationCreateDtoValidator()
        {
            RuleFor(x => x.School_University)
                .NotEmpty().WithMessage("School&University is required.")
                .MinimumLength(3).WithMessage("School&University must be at least 3 characters long.")
                .MaximumLength(500).WithMessage("School&University must not exceed 500 characters.")
                .Matches(@"^[a-zA-Z0-9\s]*$").WithMessage("School&University can only contain letters, numbers, and spaces");

            RuleFor(x => x.From)
                .NotEmpty().WithMessage("From date is required.");

            RuleFor(x => x.To)
                .NotEmpty().WithMessage("To date is required.")
                .GreaterThanOrEqualTo(x => x.From).WithMessage("To date must be after or equal to From date.");

            RuleFor(x => x.Degree)
                .NotEmpty().WithMessage("Degree is required.")
                .MinimumLength(3).WithMessage("Degree must be at least 3 characters long.")
                .MaximumLength(500).WithMessage("Degree must not exceed 500 characters.")
                .Matches(@"^[a-zA-Z0-9\s]*$").WithMessage("Degree can only contain letters, numbers, and spaces");
        }
    }
}
