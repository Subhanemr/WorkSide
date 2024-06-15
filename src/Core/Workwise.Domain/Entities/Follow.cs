namespace Workwise.Domain.Entities
{
    public class Follow : BaseEntity
    {
        public string AppUserId { get; set; } = null!;
        public AppUser? AppUser { get; set; }
    }
}
