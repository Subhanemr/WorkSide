namespace Workwise.Application.Dtos
{
    public record JobUpdateDto
    {
        public string Id { get; init; } = null!;
        public string Name { get; init; } = null!;
        public string Skill { get; init; } = null!;
        public double Price { get; init; }
        public string Time { get; init; } = null!;
        public string? Description { get; init; }

        public string CategoryId { get; init; } = null!;
        public CategoryIncludeDto? Category { get; init; }
    }
}
