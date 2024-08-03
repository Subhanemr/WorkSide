namespace Workwise.Application.Dtos
{
    public record ProjectLikeDto
    {
        public string Id { get; init; } = null!;
        public DateTime CreateAt { get; init; }
        public DateTime? UpdateAt { get; init; }
        public string CreatedBy { get; init; } = null!;

        public string ProjectId { get; init; } = null!;
        public ProjectIncludeDto? Project { get; init; }

        public string AppUserId { get; init; } = null!;
        public AppUserIncludeDto? AppUser { get; init; }
    }
}
