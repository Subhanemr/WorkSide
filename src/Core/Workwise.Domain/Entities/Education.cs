using System.Reflection.Metadata.Ecma335;

namespace Workwise.Domain.Entities
{
    public class Education : BaseEntity
    {
        public string AppUserId { get; set; } = null!;
        public AppUser? AppUser { get; set; }

        public string School_University { get; set; } = null!;
        public DateOnly From { get; set; }
        public DateOnly To { get; set; }
        public string Degree { get; set; } = null!;
        public string? Description { get; set; }
    }
}
