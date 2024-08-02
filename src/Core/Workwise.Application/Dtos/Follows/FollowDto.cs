using Workwise.Domain.Entities;

namespace Workwise.Application.Dtos
{
    public record FollowDto
    {
        public string Id { get; init; } = null!;
        public string FollowerId { get; set; } = null!;
        public string FollowingId { get; set; } = null!;

        public DateTime CreateAt { get; init; }
        public DateTime? UpdateAt { get; init; }
        public string CreatedBy { get; init; } = null!;

        public AppUserIncludeDto? Follower { get; set; }
        public AppUserIncludeDto? Following { get; set; }
    }
}
