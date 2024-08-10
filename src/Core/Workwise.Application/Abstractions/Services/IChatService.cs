using Workwise.Application.Dtos;
using Workwise.Application.Dtos.Chats;

namespace Workwise.Application.Abstractions.Services
{
    public interface IChatService
    {
        Task<ResultDto> CreateAsync(string user2Id);
        Task<ChatGetDto> GetByIdAsync(string id);
        Task<PaginationDto<ChatItemDto>> GetAll();
    }
}
