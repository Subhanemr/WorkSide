namespace Workwise.Application.Dtos.Account
{
    public record AppUserIncludeDto
    {
        public string Id { get; init; } = null!;
        public string Name { get; init; } = null!;
        public string Surname { get; init; } = null!;
        public string UserName { get; init; } = null!;
        public string Email { get; init; } = null!;

        public string? FaceLink { get; init; }
        public string? TwitLink { get; init; }
        public string? GoogleLink { get; init; }
        public string? LinkedLink { get; init; }
        public string? InstaLink { get; init; }
        public string? BehanceLink { get; init; }
        public string? YoutubeLink { get; init; }
        public bool IsOnlinne { get; init; }
        public string? CustomStatus { get; init; }
    }
}
