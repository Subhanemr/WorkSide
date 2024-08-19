namespace Workwise.Application.Dtos.Notifications
{
    public class NotificationCreateDto
    {
        public string AppUserId { get; init; } = null!;
        public string Subject { get; init; } = null!;
        public string Title { get; init; } = null!;
    }
}
