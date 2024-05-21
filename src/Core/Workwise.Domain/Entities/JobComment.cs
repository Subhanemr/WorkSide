namespace Workwise.Domain.Entities
{
    public class JobComment : BaseEntity
    {
        public string Comment { get; set; } = null!;
        public string AppUserId { get; set; } = null!;
        public AppUser? AppUser { get; set; }
        public string JobId { get; set; } = null!;
        public Job? Job { get; set; }
        public ICollection<JobReply>? JobReplies { get; set; }
    }
}
