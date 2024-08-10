using Microsoft.AspNetCore.Identity;

namespace Workwise.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;

        public bool IsActivate { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpireAt { get; set; }
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
        public bool IsOnlinne { get; set; }
        public string? CustomStatus { get; set; }

        public ICollection<Follow>? Followers { get; set; }
        public ICollection<Follow>? Followings { get; set; }
        public ICollection<Skill>? Skills { get; set; }
        public ICollection<Education>? Educations { get; set; }
        public ICollection<Experience>? Experiences { get; set; }
        public ICollection<Portfolio>? Portfolios { get; set; }
        public ICollection<Location>? Locations { get; set; }
        public ICollection<HireAccount>? HireAccounts { get; set; }
        public ICollection<Job>? Jobs { get; set; }
        public ICollection<Project>? Projects { get; set; }
        public ICollection<JobComment>? JobComments { get; set; }
        public ICollection<JobLike>? JobLikes { get; set; }
        public ICollection<JobReply>? JobReplies { get; set; }
        public ICollection<ProjectComment>? ProjectComments { get; set; }
        public ICollection<ProjectReply>? ProjectReplies { get; set; }
        public ICollection<ProjectLike>? ProjectLikes { get; set; }

        public ICollection<Notification>? Notifications { get; set; }
        public ICollection<Chat>? Chats { get; set; }
        public ICollection<Message>? Messages { get; set; }
    }
}
