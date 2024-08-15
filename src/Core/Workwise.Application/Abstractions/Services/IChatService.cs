using Workwise.Application.Dtos;
using Workwise.Application.Dtos.Chats;

namespace Workwise.Application.Abstractions.Services
{
    public interface IChatService
    {
        Task<ResultDto> CreateAsync(string user2Id);
        Task<ChatGetDto> GetByIdAsync(string id);
        Task<PaginationDto<ChatItemDto>> GetAllFilteredAsync(string? search, int take, int page, int order, bool isDeleted = false);
        Task<ResultDto> SoftDeleteAsync(string id);
        Task<ResultDto> ReverseSoftDeleteAsync(string id);
        Task<ResultDto> DeleteAsync(string id);
    }
}
