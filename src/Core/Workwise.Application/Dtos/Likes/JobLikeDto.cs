namespace Workwise.Application.Dtos
{
    public record JobLikeDto
    {
        public string Id { get; init; } = null!;
        public DateTime CreateAt { get; init; }
        public DateTime? UpdateAt { get; init; }
        public string CreatedBy { get; init; } = null!;

        public string JobId { get; init; } = null!;
        public JobIncludeDto? Jobs { get; init; }

        public string AppUserId { get; init; } = null!;
        public AppUserIncludeDto? AppUser { get; init; }
    }
}
