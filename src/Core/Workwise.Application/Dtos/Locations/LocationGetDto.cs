using Workwise.Application.Dtos.Account;

namespace Workwise.Application.Dtos
{
    public record LocationGetDto
    {
        public string Id { get; init; } = null!;
        public string Country { get; init; } = null!;
        public string City { get; init; } = null!;

        public string AppUserId { get; set; } = null!;
        public AppUserIncludeDto? AppUser { get; set; }
    }
}
