namespace Workwise.Application.Dtos.Hubs
{
    public record UserConnectionDto
    {
        public string AppUserId { get; init; }
        public List<string>? ConnectionIds { get; init; }
    }
}
