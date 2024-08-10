namespace Workwise.Domain.Entities
{
    public class Notification : BaseEntity
    {
        public string Subject { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string AppUserId { get; set; } = null!;
        public AppUser? AppUser { get; set; } 
        public bool IsRead { get; set; } = false;
    }
}
