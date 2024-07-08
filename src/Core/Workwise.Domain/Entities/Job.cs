namespace Workwise.Domain.Entities
{
    public class Job : BaseNameEntity
    {
        public string Skill { get; set; } = null!;
        public double Price { get; set; }
        public string Time { get; set; } = null!;
        public string? Description { get; set; }

        public string CategoryId { get; set; } = null!;
        public Category? Category { get; set; }

        public string AppUserId { get; set; } = null!;
        public AppUser? AppUser { get; set; }

        public ICollection<JobComment>? JobComments { get; set; }
        public ICollection<JobLike>? JobLikes { get; set; }
    }
}
