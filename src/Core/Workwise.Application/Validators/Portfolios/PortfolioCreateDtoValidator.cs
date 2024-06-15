using FluentValidation;
using Workwise.Application.Dtos;

namespace Workwise.Application.Validators.Portfolios
{
    public class PortfolioCreateDtoValidator : AbstractValidator<PortfolioCreateDto>
    {
        public PortfolioCreateDtoValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .Matches(@"^[a-zA-Z0-9\s,\.]*$").WithMessage("Name can only contain letters, numbers, spaces, commas, and periods");

            RuleFor(x => x.Url)
                .NotEmpty().WithMessage("Url is required");

            RuleFor(x => x.Link)
                .NotEmpty().WithMessage("Link is required")
                .Must(BeAValidUrl).WithMessage("Link is not a valid URL");
        }
        private bool BeAValidUrl(string url)
        {
            if (Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult))
            {
                return uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps;
            }
            return false;
        }
    }
}
