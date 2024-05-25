using Workwise.Domain.Entities;

namespace Workwise.Application.Dtos
{
    public record ProjectItemDto
    {
        public string Id { get; init; } = null!;
        public string Name { get; init; } = null!;
        public DateTime CreateAt { get; init; }
        public DateTime? UpdateAt { get; init; }
        public string CreatedBy { get; init; } = null!;
        public string Skill { get; init; } = null!;
        public double Price { get; init; }
        public double PriceTo { get; init; }
        public string? Description { get; init; }

        public string AppUserId { get; set; } = null!;
        public AppUser? AppUser { get; set; }
    }
}
