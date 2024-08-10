namespace Workwise.Domain.Entities
{
    public class Chat : BaseEntity
    {
        public string AppUser1Id { get; set; } = null!;
        public AppUser? AppUser1 { get; set; }
        public string AppUser2Id { get; set; } = null!;
        public AppUser? AppUser2 { get; set; }
        public ICollection<Message>? Messages { get; set; }
    }
}
