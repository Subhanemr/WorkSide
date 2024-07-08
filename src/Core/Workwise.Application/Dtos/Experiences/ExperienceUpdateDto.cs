namespace Workwise.Application.Dtos
{
    public record ExperienceUpdateDto
    {
        public string Id { get; init; } = null!;
        public string About { get; init; } = null!;
    }
}
