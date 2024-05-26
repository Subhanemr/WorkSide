using Workwise.Domain.Entities;

namespace Workwise.Application.Dtos
{
    public record SkillIncludeDto
    {
        public string Id { get; init; } = null!;
        public string Name { get; init; } = null!;
        public string AppUserId { get; init; } = null!;
    }
}
