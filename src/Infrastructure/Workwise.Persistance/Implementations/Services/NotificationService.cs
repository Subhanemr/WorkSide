using Workwise.Application.Abstractions.Services;
using Workwise.Application.Dtos;
using Workwise.Application.Dtos.Notifications;

namespace Workwise.Persistance.Implementations.Services
{
    public class NotificationService : INotificationService
    {
        public Task<ResultDto> CreateAsync(NotificationItemDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<ResultDto> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ResultDto> SoftDeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ResultDto> ReverseSoftDeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<NotificationGetDto>> GetAllFilteredAsync()
        {
            throw new NotImplementedException();
        }

        public Task<NotificationGetDto> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ResultDto> ReadAllNotificationsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResultDto> ReadNotificationAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
