namespace Workwise.Application.Dtos
{
    public record EducationCreateDto
    {
        public string School_University { get; init; } = null!;
        public DateOnly From { get; init; }
        public DateOnly To { get; init; }
        public string Degree { get; init; } = null!;
        public string? Description { get; init; }
    }
}
