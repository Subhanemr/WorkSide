namespace Workwise.Application.Dtos
{
    public record ForgetPasswordTokenDto
    {
        public string AppUserId { get; init; } = null!;
        public string Token { get; init; } = null!;
    }
}
