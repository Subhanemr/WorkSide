using AutoMapper;
using Workwise.Application.Dtos;
using Workwise.Domain.Entities;

namespace Workwise.Application.MappingProfiles
{
    internal class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<Project, ProjectCreateDto>().ReverseMap();
            CreateMap<Project, ProjectGetDto>().ReverseMap();
            CreateMap<Project, ProjectIncludeDto>().ReverseMap();
            CreateMap<Project, ProjectItemDto>().ReverseMap();
            CreateMap<Project, ProjectUpdateDto>().ReverseMap();

            CreateMap<ProjectComment, ProjectCommentDto>().ReverseMap();
            CreateMap<ProjectReply, ProjectReplyDto>().ReverseMap();
            CreateMap<ProjectLike, ProjectLikeDto>().ReverseMap();
        }
    }
}
