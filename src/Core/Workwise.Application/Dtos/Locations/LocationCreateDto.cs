namespace Workwise.Application.Dtos
{
    public record LocationCreateDto
    {
        public string Country { get; init; } = null!;
        public string City { get; init; } = null!;
    }
}
