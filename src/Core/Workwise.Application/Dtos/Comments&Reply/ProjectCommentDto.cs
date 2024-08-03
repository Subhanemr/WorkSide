namespace Workwise.Application.Dtos
{
    public record ProjectCommentDto
    {
        public string Id { get; init; } = null!;
        public string Comment { get; init; } = null!;
        public DateTime CreateAt { get; init; }
        public DateTime? UpdateAt { get; init; }
        public string CreatedBy { get; init; } = null!;

        public string AppUserId { get; init; } = null!;
        public AppUserIncludeDto? AppUser { get; init; }

        public string ProjectId { get; init; } = null!;
        public ProjectIncludeDto? Project { get; init; }
        public ICollection<ProjectReplyDto>? ProjectReplies { get; init; }
    }
}
