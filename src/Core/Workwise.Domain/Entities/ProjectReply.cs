namespace Workwise.Domain.Entities
{
    public class ProjectReply : BaseEntity
    {
        public string Comment { get; set; } = null!;
        public string AppUserId { get; set; } = null!;
        public AppUser? AppUser { get; set; }
        public string ProjectCommentId { get; set; } = null!;
        public ProjectComment? ProjectComment { get; set; }
    }
}
