namespace Workwise.Domain.Entities
{
    public class Location : BaseEntity
    {
        public string AppUserId { get; set; } = null!;
        public AppUser? AppUser { get; set; }

        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
    }
}
