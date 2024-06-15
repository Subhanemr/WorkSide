namespace Workwise.Domain.Entities
{
    public class Project : BaseNameEntity
    {
        public string Skill { get; set; } = null!;
        public double Price { get; set; }
        public double PriceTo { get; set; }
        public string? Description { get; set; }

        public string CategoryId { get; set; } = null!;
        public Category? Category { get; set; }
        public string AppUserId { get; set; } = null!;
        public AppUser? AppUser { get; set; }

        public ICollection<ProjectComment>? ProjectComments { get; set; }
        public ICollection<ProjectLike>? ProjectLikes { get; set; }
    }
}
