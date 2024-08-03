namespace Workwise.Application.Dtos
{
    public record AddJobReplyDto
    {
        public string Comment { get; init; } = null!;
        public string JobId { get; init; } = null!;
        public string JobCommentId { get; init; } = null!;
    }
}
