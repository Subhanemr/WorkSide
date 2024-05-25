namespace Workwise.Application.Dtos
{
    public record ProjectUpdateDto
    {
        public string Name { get; init; } = null!;
        public string Skill { get; init; } = null!;
        public double Price { get; init; }
        public double PriceTo { get; init; }
        public string? Description { get; init; }
    }
}
