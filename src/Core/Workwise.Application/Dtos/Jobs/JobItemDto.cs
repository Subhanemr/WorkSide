namespace Workwise.Application.Dtos
{
    public record JobItemDto
    {
        public string Id { get; init; } = null!;
        public string Name { get; init; } = null!;
        public string Skill { get; init; } = null!;
        public double Price { get; init; }
        public string Time { get; init; } = null!;
        public string? Description { get; init; }
        public DateTime CreateAt { get; init; }
        public DateTime? UpdateAt { get; init; }
        public string CreatedBy { get; init; } = null!;

        public string AppUserId { get; init; } = null!;
        public AppUserIncludeDto? AppUser { get; init; }

        public ICollection<JobCommentDto>? JobComments { get; init; }
        public ICollection<JobLikeDto>? JobLikes { get; init; }
    }
}
