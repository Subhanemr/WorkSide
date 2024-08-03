namespace Workwise.Application.Dtos
{
    public record JobCommentDto
    {
        public string Id { get; init; } = null!;
        public string Comment { get; init; } = null!;
        public DateTime CreateAt { get; init; }
        public DateTime? UpdateAt { get; init; }
        public string CreatedBy { get; init; } = null!;
        public string AppUserId { get; init; } = null!;
        public AppUserIncludeDto? AppUser { get; init; }
        public string JobId { get; init; } = null!;
        public JobIncludeDto? Job { get; init; }
        public ICollection<JobReplyDto>? JobReplies { get; init; }
    }
}
