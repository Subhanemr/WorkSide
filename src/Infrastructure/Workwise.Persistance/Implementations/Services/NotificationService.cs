using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Workwise.Application.Abstractions.Repositories;
using Workwise.Application.Abstractions.Services;
using Workwise.Application.Dtos;
using Workwise.Application.Dtos.Notifications;
using Workwise.Domain.Entities;
using Workwise.Persistance.Implementations.Hubs;
using Workwise.Persistance.Utilities;

namespace Workwise.Persistance.Implementations.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repository;
        private readonly IHubContext<NotificationHub> _notificationHub;
        private readonly IHttpContextAccessor _http;
        private readonly IMapper _mapper;

        public NotificationService(INotificationRepository repository, IHubContext<NotificationHub> notificationHub, IHttpContextAccessor http, 
            IMapper mapper)
        {
            _repository = repository;
            _notificationHub = notificationHub;
            _http = http;
            _mapper = mapper;
        }

        public async Task<ResultDto> CreateAsync(NotificationCreateDto dto)
        {
            string userId = _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId != dto.AppUserId || userId == null)
                throw new InvalidInputException($"User is not authorized or incorrect user.");

            Notification notification = _mapper.Map<Notification>(dto);
            await _repository.AddAsync(notification);
            await _repository.SaveChangeAsync();

            var connectionIds = NotificationHub.Connections.FirstOrDefault(x => x.AppUserId == dto.AppUserId)?.ConnectionIds;
            if (connectionIds is not null)
            {
                foreach (var connectionId in connectionIds)
                {
                    await _notificationHub.Clients.Client(connectionId).SendAsync("ReceiveNotificationMessage", dto);
                }
            }

            return new("Notification successfully sended");
        }

        public async Task<ResultDto> SoftDeleteAsync(string id)
        {
            string currentUserId = _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            Notification notification = await _getByIdAsync(id, currentUserId);

            bool isAdmin = _http.HttpContext.User.IsInRole("Admin");
            bool isModerator = _http.HttpContext.User.IsInRole("Moderator");
            if (notification.AppUserId != currentUserId || !isAdmin || !isModerator)
                throw new WrongRequestException("You do not have permission to restore this job.");

            _repository.SoftDelete(notification);
            await _repository.SaveChangeAsync();

            return new($"{notification.Subject} Notification has been permanently deleted!");
        }

        public async Task<ResultDto> ReverseSoftDeleteAsync(string id)
        {
            string currentUserId = _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            Notification notification = await _getByIdAsync(id, currentUserId);

            bool isAdmin = _http.HttpContext.User.IsInRole("Admin");
            bool isModerator = _http.HttpContext.User.IsInRole("Moderator");
            if (notification.AppUserId != currentUserId || !isAdmin || !isModerator)
                throw new WrongRequestException("You do not have permission to restore this job.");

            _repository.ReverseSoftDelete(notification);
            await _repository.SaveChangeAsync();

            return new($"{notification.Subject} Notification has been permanently deleted!");
        }

        public async Task<ResultDto> DeleteAsync(string id)
        {
            string currentUserId = _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            Notification notification = await _getByIdAsync(id, currentUserId);

            _repository.Delete(notification);
            await _repository.SaveChangeAsync();

            return new($"{notification.Subject} Notification has been permanently deleted!");
        }
        public async Task<PaginationDto<NotificationItemDto>> GetAllFilteredAsync(string? search, int take, int page, int order, bool isDeleted = false)
        {
            if (page <= 0)
                throw new WrongRequestException("Invalid page number.");
            if (take <= 0)
                throw new WrongRequestException("Invalid take value.");
            if (order <= 0)
                throw new WrongRequestException("Invalid order value.");

            string userId = _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            double count = await _repository
                .CountAsync(x => !string.IsNullOrEmpty(search) ? x.Subject.ToLower().Contains(search.ToLower()) : true, isDeleted);

            string[] includes = { $"{nameof(Notification.AppUser)}" };
            ICollection<Notification> notifications = new List<Notification>();

            switch (order)
            {
                case 1:
                    notifications = await _repository
                     .GetAllWhereByOrder(x => (!string.IsNullOrEmpty(search) ? x.Subject.ToLower().Contains(search.ToLower()) : true)
                            &&(userId != null ? x.AppUserId == userId : true),
                            x => x.CreateAt, false, isDeleted, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
                case 2:
                    notifications = await _repository
                     .GetAllWhereByOrder(x => (!string.IsNullOrEmpty(search) ? x.Subject.ToLower().Contains(search.ToLower()) : true)
                            && (userId != null ? x.AppUserId == userId : true),
                            x => x.CreateAt, true, isDeleted, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
            }

            ICollection<NotificationItemDto> dtos = _mapper.Map<ICollection<NotificationItemDto>>(notifications);

            return new()
            {
                Take = take,
                Search = search,
                Order = order,
                CurrentPage = page,
                TotalPage = Math.Ceiling(count / take),
                Items = dtos
            };
        }

        public async Task<NotificationGetDto> GetByIdAsync(string id)
        {
            string userId = _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            Notification? notification = await _getByIdAsync(id, userId, false, $"{nameof(Notification.AppUser)}");

            NotificationGetDto dto = _mapper.Map<NotificationGetDto>(notification);

            return dto;
        }

        public async Task<ResultDto> ReadAllNotificationsAsync()
        {
            string userId = _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            List<Notification> notifications = await _repository.GetAllWhereByOrder(x => x.AppUserId == userId && !x.IsRead).ToListAsync();
            notifications.ForEach(x =>
            {
                if (x.AppUserId != userId)
                    throw new WrongRequestException("You do not have permission to restore this job.");
                x.IsRead = true;
                _repository.Update(x);
            });
            await _repository.SaveChangeAsync();

            return new("All Notification are readed!");
        }

        public async Task<ResultDto> ReadNotificationAsync(string id)
        {
            string userId = _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            Notification? notification = await _getByIdAsync(id, userId);

            if(notification.AppUserId != userId)
                throw new WrongRequestException("You do not have permission to restore this job.");

            notification.IsRead = true;
            _repository.Update(notification);

            return new("Notificiation is successfully readed!");
        }

        private async Task<Notification?> _getByIdAsync(string id, string userId, bool isTracking = true, params string[] includes)
        {
            if (string.IsNullOrEmpty(id))
                throw new WrongRequestException("The provided id is null or empty");
            if (string.IsNullOrEmpty(userId))
                throw new UnAuthorizedException($"User is not authorized.");
            Notification notification = await _repository.GetByExpressionAsync(x => x.Id == id && x.AppUserId == userId, isTracking, includes);
            if (notification is null)
                throw new NotFoundException($"{id}-Notification is not found!");
            return notification;
        }
    }
}
