using AutoMapper;
using Workwise.Application.Dtos;
using Workwise.Domain.Entities;

namespace Workwise.Application.MappingProfiles
{
    internal class SkillProfile : Profile
    {
        public SkillProfile()
        {
            CreateMap<Skill, SkillCreateDto>().ReverseMap();
            CreateMap<Skill, SkillGetDto>().ReverseMap();
            CreateMap<Skill, SkillIncludeDto>().ReverseMap();
            CreateMap<Skill, SkillItemDto>().ReverseMap();
            CreateMap<Skill, SkillUpdateDto>().ReverseMap();
        }
    }
}
