using Workwise.Application.Dtos.Chats;

namespace Workwise.Application.Dtos.Messages
{
    public record MessageGetDto
    {
        public string AppUserId { get; init; } = null!;
        public AppUserIncludeDto? AppUser { get; init; }
        public string Body { get; init; } = null!;
        public DateTime CreateAt { get; init; }
        public string? FilePath { get; init; }
        public string ChatId { get; init; } = null!;
        public ChatIncludeDto? Chat { get; init; }
    }
}
