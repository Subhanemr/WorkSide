using Workwise.Application.Dtos.Follows;

namespace Workwise.Application.Dtos
{
    public record AppUserDto
    {
        public string Id { get; init; } = null!;
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
        public bool IsOnlinne { get; init; }
        public string? CustomStatus { get; init; }

        public List<FollowDto>? Follows { get; init; }
        public List<FollowingDto>? Followings { get; init; }
        public List<SkillIncludeDto>? Skills { get; init; }
        public List<EducationIncludeDto>? Educations { get; init; }
        public List<ExperienceIncludeDto>? Experiences { get; init; }
        public List<PortfolioIncludeDto>? Portfolios { get; init; }
        public List<LocationIncludeDto>? Locations { get; init; }
        public List<HireAccountDto>? HireAccounts { get; init; }
        public List<JobIncludeDto>? Jobs { get; init; }
        public List<ProjectIncludeDto>? Projects { get; init; }
    }
}
