using FluentValidation;
using Workwise.Application.Dtos;

namespace Workwise.Application.Validators.Locations
{
    public class LocationCreateDtoValidator : AbstractValidator<LocationCreateDto>
    {
        public LocationCreateDtoValidator()
        {
            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country is required.")
                .MinimumLength(3).WithMessage("Country must be at least 3 characters long.")
                .MaximumLength(500).WithMessage("Country must not exceed 500 characters.")
                .Matches(@"^[a-zA-Z\s,\.]*$").WithMessage("Country can only contain letters, spaces, commas, and periods");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required.")
                .MinimumLength(3).WithMessage("City must be at least 3 characters long.")
                .MaximumLength(500).WithMessage("City must not exceed 500 characters.")
                .Matches(@"^[a-zA-Z\s,\.]*$").WithMessage("City can only contain letters, spaces, commas, and periods");
        }
    }
}
