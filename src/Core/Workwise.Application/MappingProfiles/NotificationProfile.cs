using AutoMapper;
using Workwise.Application.Dtos.Chats;
using Workwise.Application.Dtos.Notifications;
using Workwise.Domain.Entities;

namespace Workwise.Application.MappingProfiles
{
    internal class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, NotificationGetDto>().ReverseMap();
            CreateMap<Notification, NotificationIncludeDto>().ReverseMap();
            CreateMap<Notification, NotificationItemDto>().ReverseMap();
        }
    }
}
