using AutoMapper;
using Workwise.Application.Dtos;
using Workwise.Domain.Entities;

namespace Workwise.Application.MappingProfiles
{
    internal class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<AppUser, LoginDto>().ReverseMap();
            CreateMap<AppUser, RegisterDto>().ReverseMap();
            CreateMap<AppUser, AppUserDto>().ReverseMap();
            CreateMap<AppUser, AppUserIncludeDto>().ReverseMap();
        }
    }
}
