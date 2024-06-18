namespace Workwise.Application.Dtos
{
    public record EmailConfirmTokenDto
    {
        public string Token { get; init; } = null!;
    }
}
