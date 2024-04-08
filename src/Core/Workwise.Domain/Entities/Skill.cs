namespace Workwise.Domain.Entities
{
    public class Skill : BaseNameEntity
    {
        public string AppUserId { get; set; } = null!;
        public AppUser? AppUser { get; set; }
    }
}
