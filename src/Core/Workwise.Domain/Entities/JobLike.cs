namespace Workwise.Domain.Entities
{
    public class JobLike : BaseEntity
    {
        public string JobId { get; set; } = null!;
        public Job? Jobs { get; set; }

        public string AppUserId { get; set; } = null!;
        public AppUser? AppUser { get; set; }
    }
}
