namespace Workwise.Application.Dtos
{
    public record ChangeRoleDto
    {
        public string AppUserId { get; init; } = null!;
        public string Role { get; init; } = null!;
    }
}
