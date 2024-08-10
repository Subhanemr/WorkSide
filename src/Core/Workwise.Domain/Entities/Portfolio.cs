namespace Workwise.Domain.Entities
{
    public class Portfolio : BaseNameEntity
    {
        public string? Url { get; set; }
        public string Link { get; set; } = null!;

        public string AppUserId { get; set; } = null!;
        public AppUser? AppUser { get; set; }
    }
}
