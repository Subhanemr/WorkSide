using Workwise.Application.Dtos.Chats;

namespace Workwise.Application.Dtos.Messages
{
    public record MessageIncludeDto
    {
        public string AppUserId { get; init; } = null!;
        public string Body { get; init; } = null!;
        public DateTime CreateAt { get; init; }
        public string? FilePath { get; init; }
        public string ChatId { get; init; } = null!;
    }
}
