using Workwise.Application.Dtos.Follows;

namespace Workwise.Application.Dtos
{
    public record AppUserUpdateDto
    {
        public string Name { get; init; } = null!;
        public string Surname { get; init; } = null!;
        public string UserName { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string ProfileUrl { get; init; } = null!;
        public string BanerUrl { get; init; } = null!;
        public string Specialty { get; init; } = null!;
        public string? Overview { get; init; }

        public string? FaceLink { get; init; }
        public string? TwitLink { get; init; }
        public string? GoogleLink { get; init; }
        public string? LinkedLink { get; init; }
        public string? InstaLink { get; init; }
        public string? BehanceLink { get; init; }
        public string? YoutubeLink { get; init; }
    }
}
