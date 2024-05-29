using AutoMapper;
using Workwise.Application.Dtos;
using Workwise.Domain.Entities;

namespace Workwise.Application.MappingProfiles
{
    internal class HireAccountProfile : Profile
    {
        public HireAccountProfile()
        {
            CreateMap<HireAccount, HireAccountDto>().ReverseMap();
        }
    }
}
