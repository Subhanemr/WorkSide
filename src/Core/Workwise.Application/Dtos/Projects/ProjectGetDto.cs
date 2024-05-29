using Workwise.Domain.Entities;

namespace Workwise.Application.Dtos
{
    public record ProjectGetDto
    {
        public string Id { get; init; } = null!;
        public string Name { get; init; } = null!;
        public string Skill { get; init; } = null!;
        public double Price { get; init; }
        public double PriceTo { get; init; }
        public string? Description { get; init; }
        public DateTime CreateAt { get; init; }
        public DateTime? UpdateAt { get; init; }
        public string CreatedBy { get; init; } = null!;

        public string AppUserId { get; init; } = null!;
        public AppUserIncludeDto? AppUser { get; init; }

        public ICollection<ProjectComment>? ProjectComments { get; init; }
        public ICollection<ProjectLikeDto>? ProjectLikes { get; init; }
    }
}
