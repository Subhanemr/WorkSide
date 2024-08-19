using Microsoft.AspNetCore.Http;

namespace Workwise.Application.Dtos.Messages
{
    public record MessageUpdateDto
    {
        public string Id { get; init; } = null!;
        public string ChatId { get; init; } = null!;
        public string Body { get; init; } = null!;
        public IFormFile? File { get; init; }
    }
}
