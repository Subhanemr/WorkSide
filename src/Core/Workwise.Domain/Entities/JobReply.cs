namespace Workwise.Domain.Entities
{
    public class JobReply : BaseEntity
    {
        public string ReplyComment { get; set; } = null!;
        public string AppUserId { get; set; } = null!;
        public AppUser? AppUser { get; set; }
        public string JobCommentId { get; set; } = null!;
        public JobComment? JobComment { get; set; }
    }
}
