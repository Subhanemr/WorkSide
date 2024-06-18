namespace Workwise.Application.Dtos
{
    public record ForgotPasswordDto
    {
        public string Email { get; init; } = null!;
    }
}
