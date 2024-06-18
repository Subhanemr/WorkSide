namespace Workwise.Application.Dtos
{
    public record ForgetPasswordDto
    {
        public string Email { get; init; } = null!;
    }
}
