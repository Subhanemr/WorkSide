namespace Workwise.Domain.Entities
{
    public class ProjectLike : BaseEntity
    {
        public string ProjectId { get; set; } = null!;
        public Project? Project { get; set; }

        public string AppUserId { get; set; } = null!;
        public AppUser? AppUser { get; set; }
    }
}
