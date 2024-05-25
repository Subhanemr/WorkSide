namespace Workwise.Domain.Entities
{
    public class Location : BaseEntity
    {
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;

        public string AppUserId { get; set; } = null!;
        public AppUser? AppUser { get; set; }
    }
}
