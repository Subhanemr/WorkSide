namespace Workwise.Domain.Entities
{
    public class HireAccount : BaseEntity
    {
        public string HiredAccountId { get; set; } = null!;
        public string AppUserId { get; set; } = null!;
        public AppUser? HiredAccount { get; set; }
        public AppUser? AppUser { get; set; }
    }
}
