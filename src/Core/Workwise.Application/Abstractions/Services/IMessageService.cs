using Workwise.Application.Dtos;
using Workwise.Application.Dtos.Messages;

namespace Workwise.Application.Abstractions.Services
{
    public interface IMessageService
    {
        Task<ResultDto> SendMessageAsync(MessageCreateDto dto);
        Task<ResultDto> DeleteAsync(string id);
        Task<ResultDto> SoftDeleteAsync(string id);
        Task<ResultDto> ReverseSoftDeleteAsync(string id);
        Task<PaginationDto<MessageItemDto>> GetAllFilteredAsync(string? search, int take, int page, int order, bool isDeleted = false);
        Task<MessageGetDto> GetByIdAsync(string id);
        Task<ResultDto> UpdateAsync(MessageUpdateDto dto);
    }
}
