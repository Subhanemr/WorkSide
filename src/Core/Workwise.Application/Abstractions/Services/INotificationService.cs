using Workwise.Application.Dtos;
using Workwise.Application.Dtos.Notifications;

namespace Workwise.Application.Abstractions.Services
{
    public interface INotificationService
    {
        Task<ResultDto> CreateAsync(NotificationItemDto dto);
        Task<ResultDto> DeleteAsync(string id);
        Task<ResultDto> SoftDeleteAsync(string id);
        Task<ResultDto> ReverseSoftDeleteAsync(string id);
        Task<List<NotificationGetDto>> GetAllFilteredAsync();
        Task<NotificationGetDto> GetByIdAsync(string id);
        Task<ResultDto> ReadNotificationAsync(int id);
        Task<ResultDto> ReadAllNotificationsAsync();
    }
}
