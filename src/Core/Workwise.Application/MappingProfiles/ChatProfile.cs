using AutoMapper;
using Workwise.Application.Dtos.Chats;
using Workwise.Domain.Entities;

namespace Workwise.Application.MappingProfiles
{
    internal class ChatProfile : Profile
    {
        public ChatProfile()
        {
            CreateMap<Chat, ChatGetDto>().ReverseMap();
            CreateMap<Chat, ChatItemDto>().ReverseMap();
            CreateMap<Chat, ChatIncludeDto>().ReverseMap();
        }
    }
}
