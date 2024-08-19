using Workwise.Application.Dtos;
using Workwise.Application.Dtos.Notifications;

namespace Workwise.Application.Abstractions.Services
{
    public interface INotificationService
    {
        Task<ResultDto> CreateAsync(NotificationCreateDto dto);
        Task<ResultDto> DeleteAsync(string id);
        Task<ResultDto> SoftDeleteAsync(string id);
        Task<ResultDto> ReverseSoftDeleteAsync(string id);
        Task<PaginationDto<NotificationItemDto>> GetAllFilteredAsync(string? search, int take, int page, int order, bool isDeleted = false);
        Task<NotificationGetDto> GetByIdAsync(string id);
        Task<ResultDto> ReadNotificationAsync(string id);
        Task<ResultDto> ReadAllNotificationsAsync();
    }
}
