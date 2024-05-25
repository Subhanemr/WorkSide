namespace Workwise.Domain.Entities
{
    public class Follow : BaseEntity
    {
        public string FollowerId { get; set; } = null!;
        public string FollowingId { get; set; } = null!;
        public AppUser? Follower { get; set; }
        public AppUser? Following { get; set; }
    }
}
