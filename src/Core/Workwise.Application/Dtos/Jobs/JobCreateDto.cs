namespace Workwise.Application.Dtos
{
    public record JobCreateDto
    {
        public string Name { get; init; } = null!;
        public string Skill { get; init; } = null!;
        public double Price { get; init; }
        public string Time { get; init; } = null!;
        public string? Description { get; init; }
    }
}
