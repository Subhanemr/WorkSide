using AutoMapper;
using Workwise.Application.Dtos.Messages;
using Workwise.Domain.Entities;

namespace Workwise.Application.MappingProfiles
{
    internal class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<Message, MessageCreateDto>().ReverseMap();
            CreateMap<Message, MessageGetDto>().ReverseMap();
            CreateMap<Message, MessageIncludeDto>().ReverseMap();
            CreateMap<Message, MessageItemDto>().ReverseMap();
            CreateMap<Message, MessageUpdateDto>().ReverseMap();
        }
    }
}
