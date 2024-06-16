namespace Workwise.Application.Dtos
{
    public record ChangePasswordDto
    {
        public string ExistPassword { get; init; } = null!;
        public string NewPassword { get; init; } = null!;
        public string ConfirmNewPassword { get; init; } = null!;
    }
}
