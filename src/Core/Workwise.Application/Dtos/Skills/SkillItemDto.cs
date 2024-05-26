namespace Workwise.Application.Dtos
{
    public record SkillItemDto
    {
        public string Id { get; init; } = null!;
        public string Name { get; init; } = null!;
        public string AppUserId { get; init; } = null!;
        public AppUserIncludeDto? AppUser { get; init; }
    }
}
