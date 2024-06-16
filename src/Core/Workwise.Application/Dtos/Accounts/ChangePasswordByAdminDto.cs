namespace Workwise.Application.Dtos
{
    public record ChangePasswordByAdminDto
    {
        public int AppUserId { get; init; }
        public string NewPassword { get; init; } = null!;
        public string ConfirmNewPassword { get; init; } = null!;
    }
}
