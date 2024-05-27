namespace Workwise.Domain.Entities
{
    public class Experience : BaseEntity
    {
        public string? About { get; set; }

        public string AppUserId { get; set; } = null!;
        public AppUser? AppUser { get; set; }
    }
}
