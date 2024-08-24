using AutoMapper;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using Workwise.Application.Abstractions.Repositories;
using Workwise.Application.Abstractions.Services;
using Workwise.Application.Dtos;
using Workwise.Application.Dtos.Chats;
using Workwise.Domain.Entities;
using Workwise.Persistance.Utilities;

namespace Workwise.Persistance.Implementations.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _repository;
        private readonly IHttpContextAccessor _http;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public ChatService(IChatRepository repository, IMapper mapper, INotificationService notificationService, 
            IHttpContextAccessor http)
        {
            _repository = repository;
            _mapper = mapper;
            _notificationService = notificationService;
            _http = http;
        }

        public async Task<ResultDto> CreateAsync(string user2Id)
        {
            string user1Id = _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (user1Id == null && user2Id == null)
                throw new UnAuthorizedException($"Both users must be identified.");
            Chat chat = new()
            {
                AppUser1Id = user1Id,
                AppUser2Id = user2Id
            };

            await _repository.AddAsync(chat);
            await _notificationService
                .CreateAsync(new()
                {
                    AppUserId = user2Id,
                    Subject = "New Chat",
                    Title = $"You have a new chat with {_http.HttpContext.User.FindFirstValue(ClaimTypes.Name)} {_http.HttpContext.User.FindFirstValue(ClaimTypes.Surname)}"
                });
            await _repository.SaveChangeAsync();

            return new($"Chat created successfull");
        }

        public async Task<ResultDto> SoftDeleteAsync(string id)
        {
            string[] includes = { $"{nameof(Chat.Messages)}" };
            Chat chat = await _getChatById(id, true, includes);

            _repository.SoftDelete(chat);
            await _repository.SaveChangeAsync();

            return new($"{chat.Id} Chat has been successfully soft deleted.");
        }

        public async Task<ResultDto> ReverseSoftDeleteAsync(string id)
        {
            string[] includes = { $"{nameof(Chat.Messages)}" };
            Chat chat = await _getChatById(id, true, includes);

            _repository.ReverseSoftDelete(chat);
            await _repository.SaveChangeAsync();

            return new($"{chat.Id} Chat has been successfully restored.");
        }

        public async Task<ResultDto> DeleteAsync(string id)
        {
            string[] includes = { $"{nameof(Chat.Messages)}" };
            Chat chat = await _getChatById(id, true, includes);

            _repository.Delete(chat);
            await _repository.SaveChangeAsync();

            return new($"{chat.Id} Chat has been permanently deleted.");
        }

        public async Task<PaginationDto<ChatItemDto>> GetAllFilteredAsync(string? search, int take, int page, int order, bool isDeleted = false)
        {
            string userId = _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                throw new NotFoundException("User is not found!");

            if (page <= 0)
                throw new WrongRequestException("Invalid page number.");
            if (take <= 0)
                throw new WrongRequestException("Invalid take value.");
            if (order <= 0)
                throw new WrongRequestException("Invalid order value.");

            double count = await _repository
                .CountAsync(x => !string.IsNullOrEmpty(search) ? x.Messages.Any(m => m.Body.ToLower().Contains(search.ToLower())) : true, isDeleted);

            string[] includes = { $"{nameof(Chat.AppUser1)}", $"{nameof(Chat.AppUser2)}", $"{nameof(Chat.Messages)}" };
            ICollection<Chat> chats = new List<Chat>();

            switch (order)
            {
                case 1:
                    chats = await _repository
                     .GetAllWhereByOrder(x => (!string.IsNullOrEmpty(search) ? x.Messages.Any(m => m.Body.ToLower().Contains(search.ToLower())) : true)
                            && (userId != null ? x.AppUser1Id == userId : true)
                            && (userId != null ? x.AppUser2Id == userId : true),
                            x => x.CreateAt, false, isDeleted, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
                case 2:
                    chats = await _repository
                     .GetAllWhereByOrder(x => (!string.IsNullOrEmpty(search) ? x.Messages.Any(m => m.Body.ToLower().Contains(search.ToLower())) : true)
                            && (userId != null ? x.AppUser1Id == userId : true)
                            && (userId != null ? x.AppUser2Id == userId : true),
                            x => x.CreateAt, true, isDeleted, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
            }

            ICollection<ChatItemDto> dtos = _mapper.Map<ICollection<ChatItemDto>>(chats);

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

        public async Task<ChatGetDto> GetByIdAsync(string id)
        {
            string[] includes = { $"{nameof(Chat.AppUser1)}", $"{nameof(Chat.AppUser2)}", $"{nameof(Chat.Messages)}" };
            Chat chat = await _getChatById(id, false, includes);
            ChatGetDto dto = _mapper.Map<ChatGetDto>(chat);
            return dto;
        }

        private async Task<Chat> _getChatById(string id, bool isTracking = true, params string[] includes)
        {
            if (string.IsNullOrEmpty(id))
                throw new WrongRequestException("The provided id is null or empty");
            Chat chat = await _repository.GetByIdAsync(id, isTracking, includes);
            if (chat is null)
                throw new NotFoundException($"{id}-Chat is not found");
            return chat;
        }
    }
}
