using FluentValidation;
using Workwise.Application.Dtos;

namespace Workwise.Application.Validators.Accounts
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.UserNameOrEmail)
            .NotEmpty().WithMessage("Username or email is required.")
            .MaximumLength(256).WithMessage("Username or email must not exceed 256 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .MaximumLength(64).WithMessage("Password must not exceed 64 characters.");

            RuleFor(x => x.IsRemembered)
                .NotNull().WithMessage("IsRemembered value is required.");
        }
    }
}
