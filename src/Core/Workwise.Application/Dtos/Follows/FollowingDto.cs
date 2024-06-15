namespace Workwise.Application.Dtos.Follows
{
    public class FollowingDto
    {
        public string Id { get; init; } = null!;
        public string AppUserId { get; set; } = null!;
        public DateTime CreateAt { get; init; }
        public DateTime? UpdateAt { get; init; }
        public string CreatedBy { get; init; } = null!;

        public AppUserIncludeDto? AppUser { get; set; }
    }
}
