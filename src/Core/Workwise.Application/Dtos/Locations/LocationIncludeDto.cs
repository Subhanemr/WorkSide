namespace Workwise.Application.Dtos
{
    public record LocationIncludeDto
    {
        public string Country { get; init; } = null!;
        public string City { get; init; } = null!;
        public DateTime CreateAt { get; init; }
        public DateTime? UpdateAt { get; init; }
        public string CreatedBy { get; init; } = null!;
    }
}
