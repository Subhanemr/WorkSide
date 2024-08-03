namespace Workwise.Application.Dtos
{
    public record AddProjectCommentDto
    {
        public string Comment { get; init; } = null!;
        public string ProjectId { get; init; } = null!;
        public string JobCommentId { get; init; } = null!;
    }
}
