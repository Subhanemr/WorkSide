namespace Workwise.Application.Dtos
{
    public record LoginDto
    {
        public string UserNameOrEmail { get; init; } = null!;
        public string Password { get; init; } = null!;
        public bool IsRemembered { get; init; }
    }
}
