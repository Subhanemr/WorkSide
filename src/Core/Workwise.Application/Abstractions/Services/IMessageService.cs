using Workwise.Application.Dtos;
using Workwise.Application.Dtos.Messages;

namespace Workwise.Application.Abstractions.Services
{
    public interface IMessageService
    {
        Task<ResultDto> SendMessageAsync(MessageCreateDto dto);
        Task<ResultDto> DeleteMessageAsync(string id);
        Task<ResultDto> SoftDeleteMessageAsync(string id);
        Task<ResultDto> ReverseSoftDeleteMessageAsync(string id);
        Task<PaginationDto<MessageItemDto>> GetAllMessageAsync(string id);
        Task<MessageGetDto> GetByIdMessageAsync(string id);
        Task<ResultDto> UpdateMessageAsync(MessageUpdateDto dto);
    }
}
