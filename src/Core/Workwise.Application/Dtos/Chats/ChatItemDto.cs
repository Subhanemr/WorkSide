using Workwise.Application.Dtos.Messages;

namespace Workwise.Application.Dtos.Chats
{
    public record ChatItemDto
    {
        public string AppUser1Id { get; set; } = null!;
        public AppUserIncludeDto? AppUser1 { get; set; }
        public string AppUser2Id { get; set; } = null!;
        public AppUserIncludeDto? AppUser2 { get; set; }
        public ICollection<MessageIncludeDto>? Messages { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
