namespace Workwise.Application.Dtos
{
    public class AppUserGetDto
    {
        public string Id { get; init; } = null!;
        public string UserName { get; init; } = null!;
        public string Name { get; init; } = null!;
        public string Surname { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string? Role { get; set; }
    }
}
