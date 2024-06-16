using FluentValidation;
using Workwise.Application.Dtos;

namespace Workwise.Application.Validators.Accounts
{
    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator()
        {
            RuleFor(x => x.ExistPassword)
                .NotEmpty().WithMessage("Existing password is required.")
                .MinimumLength(6).WithMessage("Existing password must be at least 6 characters long.")
                .MaximumLength(64).WithMessage("Existing password must not exceed 64 characters.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(6).WithMessage("New password must be at least 6 characters long.")
                .MaximumLength(64).WithMessage("New password must not exceed 64 characters.");

            RuleFor(x => x.ConfirmNewPassword)
                .NotEmpty().WithMessage("Please confirm your new password.")
                .Equal(x => x.NewPassword).WithMessage("The new password and confirmation password do not match.")
                .MaximumLength(64).WithMessage("Confirmation password must not exceed 64 characters.");
        }
    }
}
