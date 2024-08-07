﻿namespace Workwise.Application.Dtos
{
    public record PortfolioUpdateDto
    {
        public string Id { get; init; } = null!;
        public string Name { get; init; } = null!;
        public string Url { get; init; } = null!;
        public string Link { get; init; } = null!;
    }
}
