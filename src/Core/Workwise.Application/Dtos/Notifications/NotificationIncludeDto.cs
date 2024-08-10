namespace Workwise.Application.Dtos.Notifications
{
    public record NotificationIncludeDto
    {
        public string Id { get; init; } = null!;
        public string Subject { get; init; } = null!;
        public string Title { get; init; } = null!;
        public string AppUserId { get; init; } = null!;
        public DateTime CreateAt { get; init; }
        public bool IsRead { get; init; }
    }
}
