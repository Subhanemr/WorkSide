using Microsoft.AspNetCore.Http;

namespace Workwise.Application.Dtos
{
    public record PortfolioCreateDto
    {
        public string Name { get; init; } = null!;
        public IFormFile? File { get; init; }
        public string Link { get; init; } = null!;
    }
}
