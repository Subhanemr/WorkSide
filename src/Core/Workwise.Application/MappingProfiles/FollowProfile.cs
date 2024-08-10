using AutoMapper;
using Workwise.Application.Dtos;
using Workwise.Domain.Entities;

namespace Workwise.Application.MappingProfiles
{
    internal class FollowProfile : Profile
    {
        public FollowProfile()a
        {
            CreateMap<Follow, FollowDto>().ReverseMap();
            CreateMap<Follow, FollowingDto>().ReverseMap();
        }
    }
}
