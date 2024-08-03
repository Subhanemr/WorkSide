namespace Workwise.Application.Dtos
{
    public record AddProjectReplyDto
    {
        public string Comment { get; init; } = null!;
        public string ProjectId { get; init; } = null!;
        public string ProjectCommentId { get; init; } = null!;
    }
}
