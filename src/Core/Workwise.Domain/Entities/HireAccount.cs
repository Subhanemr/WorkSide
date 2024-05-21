namespace Workwise.Domain.Entities
{
    public class HireAccount : BaseEntity
    {
        public string HiredAccountId { get; set; }
        public string AppUserId { get; set; }
        public AppUser HiredAccount { get; set; }
        public AppUser AppUser { get; set; }
    }
}
