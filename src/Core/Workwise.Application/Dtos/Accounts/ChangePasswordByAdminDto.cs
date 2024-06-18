namespace Workwise.Application.Dtos
{
    public record ChangePasswordByAdminDto
    {
        public string AppUserId { get; init; } = null!;
        public string NewPassword { get; init; } = null!;
        public string ConfirmNewPassword { get; init; } = null!;
    }
}
