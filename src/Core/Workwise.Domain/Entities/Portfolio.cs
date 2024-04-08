namespace Workwise.Domain.Entities
{
    public class Portfolio : BaseEntity
    {
        public string Url { get; set; } = null!;
        public string AppUserId { get; set; } = null!;
        public AppUser? AppUser { get; set; }
    }
}
