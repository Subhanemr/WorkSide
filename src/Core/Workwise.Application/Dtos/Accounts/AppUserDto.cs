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

        public ICollection<FollowDto>? Followers { get; init; }
        public ICollection<FollowingDto>? Followings { get; set; }
        public ICollection<SkillIncludeDto>? Skills { get; init; }
        public ICollection<EducationIncludeDto>? Educations { get; init; }
        public ICollection<ExperienceIncludeDto>? Experiences { get; init; }
        public ICollection<PortfolioIncludeDto>? Portfolios { get; init; }
        public ICollection<LocationIncludeDto>? Locations { get; init; }
        public ICollection<HireAccountDto>? HireAccounts { get; init; }
        public ICollection<JobIncludeDto>? Jobs { get; init; }
        public ICollection<ProjectIncludeDto>? Projects { get; init; }
    }
}
