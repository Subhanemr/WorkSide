﻿namespace Workwise.Application.Dtos
{
    public record PortfolioItemDto
    {
        public string Id { get; init; } = null!;
        public string Name { get; init; } = null!;
        public string Url { get; init; } = null!;
        public string Link { get; init; } = null!;
        public DateTime CreateAt { get; init; }
        public DateTime? UpdateAt { get; init; }
        public string CreatedBy { get; init; } = null!;

        public string AppUserId { get; set; } = null!;
        public AppUserIncludeDto? AppUser { get; set; }
    }
}
