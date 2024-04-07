using Microsoft.AspNetCore.Identity;

namespace Workwise.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; } = null!;
        public string? Surname { get; set; }

        public bool IsActivate { get; set; }
        public string Img { get; set; } = "https://res.cloudinary.com/dzsysx73x/image/upload/v1707148401/cwlzdvof54s1fw1eo19z.png";
        public string? About { get; set; }

    }
}
