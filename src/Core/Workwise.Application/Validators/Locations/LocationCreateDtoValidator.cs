using FluentValidation;
using Workwise.Application.Dtos;

namespace Workwise.Application.Validators.Locations
{
    public class LocationCreateDtoValidator : AbstractValidator<LocationCreateDto>
    {
        public LocationCreateDtoValidator()
        {
            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country is required")
                .Matches(@"^[a-zA-Z\s,\.]*$").WithMessage("Country can only contain letters, spaces, commas, and periods");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required")
                .Matches(@"^[a-zA-Z\s,\.]*$").WithMessage("City can only contain letters, spaces, commas, and periods");
        }
    }
}
