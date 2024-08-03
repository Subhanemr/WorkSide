namespace Workwise.Application.Dtos
{
    public record AddJobCommentDto
    {
        public string JobId { get; init; } = null!;
        public string Comment { get; init; } = null!;
    }
}
