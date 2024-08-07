﻿using AutoMapper;
using Workwise.Application.Dtos;
using Workwise.Domain.Entities;

namespace Workwise.Application.MappingProfiles
{
    internal class JobProfile : Profile
    {
        public JobProfile()
        {
            CreateMap<Job, JobCreateDto>().ReverseMap();
            CreateMap<Job, JobGetDto>().ReverseMap();
            CreateMap<Job, JobIncludeDto>().ReverseMap();
            CreateMap<Job, JobItemDto>().ReverseMap();
            CreateMap<Job, JobUpdateDto>().ReverseMap();

            CreateMap<JobComment, JobCommentDto>().ReverseMap();
            CreateMap<JobComment, AddJobCommentDto>().ReverseMap();
            CreateMap<JobReply, JobReplyDto>().ReverseMap();
            CreateMap<JobReply, AddJobReplyDto>().ReverseMap();
            CreateMap<JobLike, JobLikeDto>().ReverseMap();
        }
    }
}
