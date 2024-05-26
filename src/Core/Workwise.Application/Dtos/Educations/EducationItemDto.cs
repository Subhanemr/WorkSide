namespace Workwise.Application.Dtos
{
    internal class EducationItemDto
    {
        public string School_University { get; init; } = null!;
        public DateOnly From { get; init; }
        public DateOnly To { get; init; }
        public string Degree { get; init; } = null!;
        public string? Description { get; init; }

        public string AppUserId { get; init; } = null!;
        public AppUserIncludeDto? AppUser { get; init; }
    }
}
