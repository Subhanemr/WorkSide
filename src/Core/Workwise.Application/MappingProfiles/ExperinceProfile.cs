using AutoMapper;
using Workwise.Application.Dtos;
using Workwise.Domain.Entities;

namespace Workwise.Application.MappingProfiles
{
    internal class ExperinceProfile : Profile
    {
        public ExperinceProfile()
        {
            CreateMap<Experience, ExperienceCreateDto>().ReverseMap();
            CreateMap<Experience, ExperienceGetDto>().ReverseMap();
            CreateMap<Experience, ExperienceIncludeDto>().ReverseMap();
            CreateMap<Experience, ExperienceItemDto>().ReverseMap();
            CreateMap<Experience, ExperienceUpdateDto>().ReverseMap();
        }
    }
}
