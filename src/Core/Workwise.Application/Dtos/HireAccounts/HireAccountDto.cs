namespace Workwise.Application.Dtos
{
    public record HireAccountDto
    {
        public string Id { get; init; } = null!;
        public string HiredAccountId { get; init; } = null!;
        public string AppUserId { get; init; } = null!;
        public DateTime CreateAt { get; init; }
        public DateTime? UpdateAt { get; init; }
        public string CreatedBy { get; init; } = null!;

        public AppUserIncludeDto? HiredAccount { get; init; }
        public AppUserIncludeDto? AppUser { get; init; }
    }
}
