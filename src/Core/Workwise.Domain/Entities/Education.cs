﻿namespace Workwise.Domain.Entities
{
    public class Education : BaseEntity
    {
        public string School_University { get; set; } = null!;
        public DateOnly From { get; set; }
        public DateOnly To { get; set; }
        public string Degree { get; set; } = null!;
        public string? Description { get; set; }

        public string AppUserId { get; set; } = null!;
        public AppUser? AppUser { get; set; }
    }
}
