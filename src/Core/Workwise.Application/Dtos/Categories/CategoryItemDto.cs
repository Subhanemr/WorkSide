namespace Workwise.Application.Dtos
{
    public record CategoryItemDto
    {
        public string Name { get; init; } = null!;
        public ICollection<JobIncludeDto>? Jobs { get; init; }
        public ICollection<ProjectIncludeDto>? Projects { get; init; }
    }
}
