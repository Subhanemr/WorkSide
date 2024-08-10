namespace Workwise.Domain.Entities
{
    public class Message : BaseEntity
    {
        public string AppUserId { get; set; } = null!;
        public AppUser? AppUser { get; set; }
        public string Body { get; set; } = null!;
        public string? FilePath { get; set; }
        public string ChatId { get; set; } = null!;
        public Chat? Chat { get; set; }
    }
}
