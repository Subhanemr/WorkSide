using Workwise.Domain.Entities;

namespace Workwise.Application.Dtos
{
    public record AppUserDto
    {
        public string Id { get; init; } = null!;
        public string Name { get; init; } = null!;
        public string Surname { get; init; } = null!;
        public string UserName { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string ProfileUrl { get; set; } = "https://res.cloudinary.com/dzsysx73x/image/upload/v1707148401/cwlzdvof54s1fw1eo19z.png";
        public string BanerUrl { get; set; } = "https://res.cloudinary.com/dzsysx73x/image/upload/v1707148401/cwlzdvof54s1fw1eo19z.png";
        public string Specialty { get; set; } = null!;
        public string? Overview { get; set; }

        public string? FaceLink { get; init; }
        public string? TwitLink { get; init; }
        public string? GoogleLink { get; init; }
        public string? LinkedLink { get; init; }
        public string? InstaLink { get; init; }
        public string? BehanceLink { get; init; }
        public string? YoutubeLink { get; init; }
        public bool IsOnlinne { get; init; }
        public string? CustomStatus { get; init; }

        public List<FollowDto>? Follows { get; set; }
        public List<Skill>? Skills { get; set; }
        public List<Education>? Educations { get; set; }
        public List<Experience>? Experiences { get; set; }
        public List<Portfolio>? Portfolios { get; set; }
        public List<LocationIncludeDto>? Locations { get; set; }
        public List<HireAccount>? HireAccounts { get; set; }
        public List<JobIncludeDto>? Jobs { get; set; }
        public List<ProjectIncludeDto>? Projects { get; set; }
    }
}
