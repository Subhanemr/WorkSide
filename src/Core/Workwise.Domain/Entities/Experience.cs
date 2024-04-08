namespace Workwise.Domain.Entities
{
    public class Experience : BaseEntity
    {
        public string AppUserId { get; set; } = null!;
        public AppUser? AppUser { get; set; }

        public string? About { get; set; }

    }
}
