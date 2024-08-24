using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Workwise.API.Utilities.Helpers;
using Workwise.Application.Abstractions.Repositories;
using Workwise.Application.Abstractions.Services;
using Workwise.Application.Dtos;
using Workwise.Application.Dtos.Chats;
using Workwise.Application.Dtos.Messages;
using Workwise.Domain.Entities;
using Workwise.Persistance.Implementations.Hubs;
using Workwise.Persistance.Utilities;

namespace Workwise.Persistance.Implementations.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _repository;
        private readonly IMapper _mapper;
        private readonly IChatService _chatService;
        private readonly IHubContext<ChatHub> _chatHub;
        private readonly ICLoudService _cLoudService;
        private readonly IHttpContextAccessor _http;

        public MessageService(IMessageRepository repository, IMapper mapper, IChatService chatService,
            IHubContext<ChatHub> chatHub, ICLoudService cLoudService, IHttpContextAccessor http)
        {
            _repository = repository;
            _mapper = mapper;
            _chatService = chatService;
            _chatHub = chatHub;
            _cLoudService = cLoudService;
            _http = http;
        }

        public async Task<ResultDto> SendMessageAsync(MessageCreateDto dto)
        {
            string userId = _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ChatGetDto chat = await _chatService.GetByIdAsync(dto.ChatId);

            Message message = _mapper.Map<Message>(dto);
            message.AppUserId = userId;
            if (dto.File != null)
            {
                dto.File.ValidateImage(5);
                message.FilePath = await _cLoudService.FileCreateAsync(dto.File);
            }

            await _repository.AddAsync(message);
            await _repository.SaveChangeAsync();

            var userConnectionIds = ChatHub.Connections.FirstOrDefault(x => x.AppUserId == chat.AppUser1Id)?.ConnectionIds;
            if (userConnectionIds is not null)
            {
                foreach (var connectionId in userConnectionIds)
                {
                    await _chatHub.Clients.Client(connectionId).SendAsync("ReceiveChatMessage", dto);
                }
            }

            var MemberConnectionIds = ChatHub.Connections.FirstOrDefault(x => x.AppUserId == chat.AppUser2Id)?.ConnectionIds;
            if (MemberConnectionIds is not null)
            {
                foreach (var connectionId in MemberConnectionIds)
                {
                    await _chatHub.Clients.Client(connectionId).SendAsync("ReceiveChatMessage", dto);
                }
            }

            return new("Message is successfully created");
        }

        public async Task<ResultDto> SoftDeleteAsync(string id)
        {
            Message message = await _getByIdAsync(id);

            string currentUserId = _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isAdmin = _http.HttpContext.User.IsInRole("Admin");
            bool isModerator = _http.HttpContext.User.IsInRole("Moderator");
            if (message.AppUserId != currentUserId || !isAdmin || !isModerator)
                throw new WrongRequestException("You do not have permission to restore this job.");

            _repository.SoftDelete(message);
            await _repository.SaveChangeAsync();

            return new($"{message.Body} Message has been permanently deleted");
        }

        public async Task<ResultDto> ReverseSoftDeleteAsync(string id)
        {
            Message message = await _getByIdAsync(id);

            string currentUserId = _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isAdmin = _http.HttpContext.User.IsInRole("Admin");
            bool isModerator = _http.HttpContext.User.IsInRole("Moderator");
            if (message.AppUserId != currentUserId || !isAdmin || !isModerator)
                throw new WrongRequestException("You do not have permission to restore this job.");

            _repository.ReverseSoftDelete(message);
            await _repository.SaveChangeAsync();

            return new($"{message.Body} Message has been permanently deleted");
        }

        public async Task<ResultDto> DeleteAsync(string id)
        {
            Message message = await _getByIdAsync(id);

            _repository.Delete(message);
            await _repository.SaveChangeAsync();

            return new($"{message.Body} Message has been permanently deleted");
        }

        public async Task<PaginationDto<MessageItemDto>> GetAllFilteredAsync(string? search, int take, int page, int order, bool isDeleted = false)
        {
            if (page <= 0)
                throw new WrongRequestException("Invalid page number.");
            if (take <= 0)
                throw new WrongRequestException("Invalid take value.");
            if (order <= 0)
                throw new WrongRequestException("Invalid order value.");

            double count = await _repository
                .CountAsync(x => !string.IsNullOrEmpty(search) ? x.Body.ToLower().Contains(search.ToLower()) : true, isDeleted);

            string[] includes = { $"{nameof(Message.AppUser)}", $"{nameof(Message.Chat)}" };
            ICollection<Message> chats = new List<Message>();

            switch (order)
            {
                case 1:
                    chats = await _repository
                     .GetAllWhereByOrder(x => (!string.IsNullOrEmpty(search) ? x.Body.ToLower().Contains(search.ToLower()) : true),
                            x => x.CreateAt, false, isDeleted, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
                case 2:
                    chats = await _repository
                     .GetAllWhereByOrder(x => (!string.IsNullOrEmpty(search) ? x.Body.ToLower().Contains(search.ToLower()) : true),
                            x => x.CreateAt, true, isDeleted, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
            }

            ICollection<MessageItemDto> dtos = _mapper.Map<ICollection<MessageItemDto>>(chats);

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

        public async Task<MessageGetDto> GetByIdAsync(string id)
        {
            string[] includes = { $"{nameof(Message.AppUser)}" };
            Message message = await _getByIdAsync(id, false, includes);

            MessageGetDto dto = _mapper.Map<MessageGetDto>(message);

            return dto;
        }

        public async Task<ResultDto> UpdateAsync(MessageUpdateDto dto)
        {
            string userId = _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ChatGetDto chat = await _chatService.GetByIdAsync(dto.ChatId);

            Message message = await _getByIdAsync(dto.Id);
            _mapper.Map(dto, message);
            message.AppUserId = userId;
            if (dto.File != null)
            {
                dto.File.ValidateImage(5);
                message.FilePath = await _cLoudService.FileCreateAsync(dto.File);
            }

            _repository.Update(message);
            await _repository.SaveChangeAsync();

            var userConnectionIds = ChatHub.Connections.FirstOrDefault(x => x.AppUserId == chat.AppUser1Id)?.ConnectionIds;
            if (userConnectionIds is not null)
            {
                foreach (var connectionId in userConnectionIds)
                {
                    await _chatHub.Clients.Client(connectionId).SendAsync("ReceiveChatMessage", dto);
                }
            }

            var MemberConnectionIds = ChatHub.Connections.FirstOrDefault(x => x.AppUserId == chat.AppUser2Id)?.ConnectionIds;
            if (MemberConnectionIds is not null)
            {
                foreach (var connectionId in MemberConnectionIds)
                {
                    await _chatHub.Clients.Client(connectionId).SendAsync("ReceiveChatMessage", dto);
                }
            }

            return new("Message is successfully created");
        }

        private async Task<Message> _getByIdAsync(string id, bool isTracking = true, params string[] includes)
        {
            if (string.IsNullOrEmpty(id))
                throw new WrongRequestException("The provided id is null or empty");
            Message message = await _repository.GetByIdAsync(id, isTracking, includes);
            if (message is null)
                throw new NotFoundException($"{id}-Chat is not found");
            return message;
        }
    }
}
