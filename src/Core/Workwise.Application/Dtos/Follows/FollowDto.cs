namespace Workwise.Application.Dtos
{
    public record FollowDto
    {
        public string Id { get; init; } = null!;
        public string FollowerId { get; init; } = null!;
        public string FollowingId { get; init; } = null!;
        public DateTime CreateAt { get; init; }
        public DateTime? UpdateAt { get; init; }
        public string CreatedBy { get; init; } = null!;

        public AppUserIncludeDto? Follower { get; init; }
        public AppUserIncludeDto? Following { get; init; }
    }
}
