using Microsoft.AspNetCore.Identity;

namespace Workwise.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; } = null!;
        public string? Surname { get; set; }

        public bool IsActivate { get; set; }
        public string ProfileUrl { get; set; } = "https://res.cloudinary.com/dzsysx73x/image/upload/v1707148401/cwlzdvof54s1fw1eo19z.png";
        public string BanerUrl { get; set; } = "https://res.cloudinary.com/dzsysx73x/image/upload/v1707148401/cwlzdvof54s1fw1eo19z.png";
        public string Specialty { get; set; } = null!;
        public string? Overview { get; set; }


        public string? FaceLink { get; set; }
        public string? TwitLink { get; set; }
        public string? GoogleLink { get; set; }
        public string? LinkedLink { get; set; }
        public string? InstaLink { get; set; }
        public string? BehanceLink { get; set; }
        public string? YoutubeLink { get; set; }


        public List<Follow>? Follows { get; set; }
        public List<Skill>? Skills { get; set; }
        public List<Education>? Educations { get; set; }
        public List<Experience>? Experiences { get; set; }
        public List<Portfolio>? Portfolios { get; set; }
        public List<Location>? Locations { get; set; }
    }
}
