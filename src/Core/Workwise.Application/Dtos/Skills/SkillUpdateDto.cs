namespace Workwise.Application.Dtos
{
    public record SkillUpdateDto
    {
        public string Id { get; init; } = null!;
        public string Name { get; init; } = null!;
    }
}
