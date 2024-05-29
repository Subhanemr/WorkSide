using AutoMapper;
using Workwise.Application.Dtos;
using Workwise.Domain.Entities;

namespace Workwise.Application.MappingProfiles
{
    internal class EducationProfile : Profile
    {
        public EducationProfile()
        {
            CreateMap<Education, EducationCreateDto>().ReverseMap();
            CreateMap<Education, EducationGetDto>().ReverseMap();
            CreateMap<Education, EducationIncludeDto>().ReverseMap();
            CreateMap<Education, EducationItemDto>().ReverseMap();
            CreateMap<Education, EducationUpdateDto>().ReverseMap();
        }
    }
}
