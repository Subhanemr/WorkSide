namespace Workwise.Application.Dtos
{
    public record CategoryUpdateDto
    {
        public string Id { get; init; } = null!;
        public string Name { get; init; } = null!;
    }
}
