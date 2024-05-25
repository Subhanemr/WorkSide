namespace Workwise.Application.Dtos
{
    public record CategoryGetDto
    {
        public string Id { get; init; } = null!;
        public string Name { get; init; } = null!;
        public DateTime CreateAt { get; init; }
        public DateTime? UpdateAt { get; init; }
        public string CreatedBy { get; init; } = null!;
        public ICollection<JobIncludeDto>? Jobs { get; init; }
        public ICollection<ProjectIncludeDto>? Projects { get; init; }

    }
}
