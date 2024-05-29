using AutoMapper;
using Workwise.Application.Dtos;
using Workwise.Domain.Entities;

namespace Workwise.Application.MappingProfiles
{
    internal class PortfolioProfile : Profile
    {
        public PortfolioProfile()
        {
            CreateMap<Portfolio, PortfolioCreateDto>().ReverseMap();
            CreateMap<Portfolio, PortfolioGetDto>().ReverseMap();
            CreateMap<Portfolio, PortfolioIncludeDto>().ReverseMap();
            CreateMap<Portfolio, PortfolioItemDto>().ReverseMap();
            CreateMap<Portfolio, PortfolioUpdateDto>().ReverseMap();
        }
    }
}
