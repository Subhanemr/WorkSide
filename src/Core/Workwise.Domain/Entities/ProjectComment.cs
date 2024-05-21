namespace Workwise.Domain.Entities
{
    public class ProjectComment : BaseEntity
    {
        public string Comment { get; set; } = null!;
        public string AppUserId { get; set; } = null!;
        public AppUser? AppUser { get; set; }
        public string ProjectId { get; set; } = null!;
        public Project? Project { get; set; }
        public ICollection<ProjectReply>? ProjectReplies { get; set; }
    }
}
