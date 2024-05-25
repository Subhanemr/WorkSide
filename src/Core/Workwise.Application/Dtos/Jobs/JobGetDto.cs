using Workwise.Domain.Entities;

namespace Workwise.Application.Dtos
{
    public record JobGetDto
    {
        public string Id { get; init; } = null!;
        public string Name { get; init; } = null!;
        public DateTime CreateAt { get; init; }
        public DateTime? UpdateAt { get; init; }
        public string CreatedBy { get; init; } = null!;
        public string Skill { get; init; } = null!;
        public double Price { get; init; }
        public string Time { get; init; } = null!;
        public string? Description { get; init; }

        public string AppUserId { get; init; } = null!;
        public AppUser? AppUser { get; init; }
    }
}
