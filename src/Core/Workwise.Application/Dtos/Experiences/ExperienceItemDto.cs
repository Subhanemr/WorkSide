namespace Workwise.Application.Dtos
{
    public record ExperienceItemDto
    {
        public string Id { get; init; } = null!;
        public string? About { get; init; }
        public DateTime CreateAt { get; init; }
        public DateTime? UpdateAt { get; init; }
        public string CreatedBy { get; init; } = null!;

        public string AppUserId { get; init; } = null!;
        public AppUserIncludeDto? AppUser { get; init; }
    }
}
