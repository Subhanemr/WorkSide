using Workwise.Application.Dtos.Messages;

namespace Workwise.Application.Dtos.Chats
{
    public record ChatIncludeDto
    {
        public string AppUser1Id { get; set; } = null!;
        public string AppUser2Id { get; set; } = null!;
        public DateTime CreateAt { get; set; }
    }
}
