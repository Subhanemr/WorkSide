namespace Workwise.Domain.Entities
{
    public class Following : BaseEntity
    {
        public string AppUserId { get; set; } = null!;
        public AppUser? AppUser { get; set; }
    }
}
