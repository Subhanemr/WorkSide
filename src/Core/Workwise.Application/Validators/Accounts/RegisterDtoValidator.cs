using FluentValidation;
using FluentValidation.Validators;
using Workwise.Application.Dtos;

namespace Workwise.Application.Validators.Accounts
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MinimumLength(3).WithMessage("Name must be at least 3 characters long.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.")
                .Matches(@"^[a-zA-Z\-'.\s]+$").WithMessage("Name can only contain letters, hyphens, apostrophes, dots, and spaces.");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Surname is required.")
                .MinimumLength(3).WithMessage("Surname must be at least 3 characters long.")
                .MaximumLength(100).WithMessage("Surname must not exceed 100 characters.")
                .Matches(@"^[a-zA-Z\-'.\s]+$").WithMessage("Surname can only contain letters, hyphens, apostrophes, dots, and spaces.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
                .MaximumLength(100).WithMessage("Username must not exceed 100 characters.")
                .Matches(@"^[a-zA-Z0-9_\-]+$").WithMessage("Username can only contain letters, numbers, underscores, and hyphens.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .MinimumLength(5).WithMessage("Email must be at least 5 characters long.")
                .MaximumLength(256).WithMessage("Email must not exceed 256 characters.")
                .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$").WithMessage("Email must be a valid email address.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .MaximumLength(64).WithMessage("Password must not exceed 64 characters.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Please confirm your password.")
                .Equal(x => x.Password).WithMessage("The password and confirmation password do not match.")
                .MaximumLength(64).WithMessage("Confirmation password must not exceed 64 characters.");
        }
    }
}
