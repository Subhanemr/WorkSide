namespace Workwise.Application.Dtos
{
    public record EducationGetDto
    {
        public string Id { get; init; } = null!;
        public string School_University { get; init; } = null!;
        public DateOnly From { get; init; }
        public DateOnly To { get; init; }
        public string Degree { get; init; } = null!;
        public string? Description { get; init; }
        public DateTime CreateAt { get; init; }
        public DateTime? UpdateAt { get; init; }
        public string CreatedBy { get; init; } = null!;

        public string AppUserId { get; init; } = null!;
        public AppUserIncludeDto? AppUser { get; init; }
    }
}
