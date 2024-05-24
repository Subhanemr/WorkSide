namespace Workwise.Application.Dtos
{
    public record CategoryGetDto
    {
        public string Name { get; init; } = null!;
        public ICollection<JobIncludeDto>? Jobs { get; init; }
        public ICollection<ProjectIncludeDto>? Projects { get; init; }

    }
}
