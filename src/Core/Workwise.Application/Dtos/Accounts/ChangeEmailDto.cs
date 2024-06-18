namespace Workwise.Application.Dtos.Accounts
{
    public record ChangeEmailDto
    {
        public string Id { get; init; } = null!;
        public string Token { get; init; } = null!;
        public string Email { get; init; } = null!;
    }
}
