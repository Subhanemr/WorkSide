using Microsoft.AspNetCore.Http;

namespace Workwise.Application.Dtos.Messages
{
    public record MessageCreateDto
    {
        public string ChatId { get; init; } = null!;
        public string Body { get; init; } = null!;
        public IFormFile? File { get; init; }
    }
}
