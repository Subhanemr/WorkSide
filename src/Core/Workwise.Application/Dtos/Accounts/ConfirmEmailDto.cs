namespace Workwise.Application.Dtos
{
    public record ConfirmEmailDto
    {
        public string AppUserId { get; init; } = null!;
        public string Token { get; init; } = null!;
    }
}
