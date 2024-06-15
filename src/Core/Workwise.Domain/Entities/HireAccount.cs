namespace Workwise.Domain.Entities
{
    public class HireAccount : BaseEntity
    {
        public string AppUserId { get; set; } = null!;
        public AppUser? AppUser { get; set; }
    }
}
