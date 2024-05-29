using AutoMapper;
using Workwise.Application.Dtos;
using Workwise.Domain.Entities;

namespace Workwise.Application.MappingProfiles
{
    internal class LocationProfile : Profile
    {
        public LocationProfile()
        {
            CreateMap<Location, LocationCreateDto>().ReverseMap();
            CreateMap<Location, LocationGetDto>().ReverseMap();
            CreateMap<Location, LocationIncludeDto>().ReverseMap();
            CreateMap<Location, LocationItemDto>().ReverseMap();
            CreateMap<Location, LocationUpdateDto>().ReverseMap();
        }
    }
}
