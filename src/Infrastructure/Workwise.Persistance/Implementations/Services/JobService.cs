using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Workwise.Application.Abstractions.Repositories;
using Workwise.Application.Abstractions.Services;
using Workwise.Application.Dtos;
using Workwise.Domain.Entities;
using Workwise.Persistance.Utilities;

namespace Workwise.Persistance.Implementations.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _repository;
        private readonly IHttpContextAccessor _http;
        private readonly IMapper _mapper;

        public JobService(IJobRepository repository, IMapper mapper, IHttpContextAccessor http)
        {
            _repository = repository;
            _mapper = mapper;
            _http = http;
        }

        public async Task<ResultDto> CreateAsync(JobCreateDto dto)
        {
            if (await _repository.CheckUniqueAsync(x => x.Name == dto.Name))
                throw new AlreadyExistException($"{dto.Name} - This Job is already exist!");

            Job item = _mapper.Map<Job>(dto);

            await _repository.AddAsync(item);
            await _repository.SaveChangeAsync();

            return new($"{dto.Name} - Job is successfully created");
        }

        public async Task<ResultDto> SoftDeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new WrongRequestException("The provided id is null or empty");

            Job item = await _getByIdAsync(id);

            string currentUserId = _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isAdmin = _http.HttpContext.User.IsInRole("Admin");
            bool isModerator = _http.HttpContext.User.IsInRole("Moderator");
            if (item.AppUserId != currentUserId || !isAdmin || !isModerator)
                throw new WrongRequestException("You do not have permission to restore this job.");

            _repository.SoftDelete(item);
            await _repository.SaveChangeAsync();

            return new($"{item.Name} Job has been successfully soft deleted");
        }

        public async Task<ResultDto> ReverseSoftDeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new WrongRequestException("The provided id is null or empty");

            Job item = await _getByIdAsync(id);

            string currentUserId = _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isAdmin = _http.HttpContext.User.IsInRole("Admin");
            bool isModerator = _http.HttpContext.User.IsInRole("Moderator");
            if (item.AppUserId != currentUserId || !isAdmin || !isModerator)
                throw new WrongRequestException("You do not have permission to restore this job.");

            _repository.ReverseSoftDelete(item);
            await _repository.SaveChangeAsync();

            return new($"{item.Name} Job has been successfully restored");
        }

        public async Task<ResultDto> DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new WrongRequestException("The provided id is null or empty");

            string[] includes = { $"{nameof(Job.Category)}", $"{nameof(Job.AppUser)}", $"{nameof(Job.JobLikes)}", $"{nameof(Job.JobComments)}.{nameof(JobComment.JobReplies)}" };
            Job item = await _getByIdAsync(id, true, includes);

            _repository.Delete(item);
            await _repository.SaveChangeAsync();

            return new($"{item.Name} Job has been permanently deleted");
        }

        public async Task<PaginationDto<JobItemDto>> GetAllFilteredAsync(string? search, int take, int page, int order, bool isDeleted = false)
        {
            if (page <= 0)
                throw new WrongRequestException("Invalid page number.");
            if (take <= 0)
                throw new WrongRequestException("Invalid take value.");
            if (order <= 0)
                throw new WrongRequestException("Invalid order value.");

            string[] includes = { $"{nameof(Job.Category)}", $"{nameof(Job.AppUser)}", $"{nameof(Job.JobLikes)}", $"{nameof(Job.JobComments)}.{nameof(JobComment.JobReplies)}" };
            double count = await _repository
                .CountAsync(x => !string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true, isDeleted);

            ICollection<Job> items = new List<Job>();

            switch (order)
            {
                case 1:
                    items = await _repository
                    .GetAllWhereByOrder(x => !string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true,
                        x => x.Name, false, isDeleted, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
                case 2:
                    items = await _repository
                     .GetAllWhereByOrder(x => !string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true,
                      x => x.CreateAt, false, isDeleted, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
                case 3:
                    items = await _repository
                    .GetAllWhereByOrder(x => !string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true,
                        x => x.Name, true, isDeleted, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
                case 4:
                    items = await _repository
                     .GetAllWhereByOrder(x => !string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true,
                      x => x.CreateAt, true, isDeleted, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
            }

            ICollection<JobItemDto> dtos = _mapper.Map<ICollection<JobItemDto>>(items);

            return new()
            {
                Take = take,
                Search = search,
                Order = order,
                CurrentPage = page,
                TotalPage = Math.Ceiling(count / take),
                Items = dtos
            };
        }

        public async Task<JobGetDto> GetByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new WrongRequestException("The provided id is null or empty");
            string[] includes = { $"{nameof(Job.Category)}", $"{nameof(Job.AppUser)}", $"{nameof(Job.JobLikes)}", $"{nameof(Job.JobComments)}.{nameof(JobComment.JobReplies)}" };
            Job item = await _getByIdAsync(id, false, includes);

            JobGetDto dto = _mapper.Map<JobGetDto>(item);
            return dto;
        }

        public async Task<ResultDto> UpdateAsync(JobUpdateDto dto)
        {
            Job existedItem = await _getByIdAsync(dto.Id);

            if (await _repository.CheckUniqueAsync(x => x.Name.ToLower() == dto.Name.ToLower().Trim() && x.Id != dto.Id))
                throw new AlreadyExistException($"{dto.Name}-This Job is already exist!");

            existedItem = _mapper.Map(dto, existedItem);
            _repository.Update(existedItem);
            await _repository.SaveChangeAsync();

            return new($"{dto.Name} - Job is successfully updated");
        }

        public async Task<ResultDto> AddCommentAsync(AddJobCommentDto dto)
        {
            string[] includes = { $"{nameof(Job.JobComments)}.{nameof(JobComment.JobReplies)}" };
            Job item = await _getByIdAsync(dto.JobId, true, includes);

            JobComment comment = _mapper.Map<JobComment>(dto);
            comment.AppUserId = _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            item.JobComments.Add(comment);

            _repository.Update(item);
            await _repository.SaveChangeAsync();

            return new($"Comment added to job with ID {dto.JobId}.");
        }

        public async Task<ResultDto> DeleteCommentAsync(string jobId, string jobCommentId)
        {
            string[] includes = { $"{nameof(Job.JobComments)}.{nameof(JobComment.JobReplies)}" };
            Job item = await _getByIdAsync(jobId, true, includes);

            JobComment? commentToRemove = item.JobComments.FirstOrDefault(x => x.Id == jobCommentId);
            if (commentToRemove == null)
                throw new NotFoundException($"Comment with ID {jobCommentId} not found in job {jobId}.");

            item.JobComments.Remove(commentToRemove);

            _repository.Update(item);
            await _repository.SaveChangeAsync();

            return new($"Comment with ID {jobCommentId} has been removed from job {jobId}.");
        }

        public async Task<ResultDto> AddReplyAsync(AddJobReplyDto dto)
        {
            string[] includes = { $"{nameof(Job.JobComments)}.{nameof(JobComment.JobReplies)}" };
            Job item = await _getByIdAsync(dto.JobId, true, includes);

            JobReply reply = _mapper.Map<JobReply>(dto);
            reply.AppUserId = _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (item.JobComments.FirstOrDefault(c => c.Id == dto.JobCommentId) == null)
                throw new NotFoundException($"Comment with ID {dto.JobCommentId} not found!");

            item.JobComments.FirstOrDefault(c => c.Id == dto.JobCommentId).JobReplies.Add(reply);

            _repository.Update(item);
            await _repository.SaveChangeAsync();

            return new($"Reply added to comment with ID {dto.JobCommentId} in job with ID {dto.JobId}.");
        }

        public async Task<ResultDto> DeleteReplyAsync(string jobId, string jobCommentId, string jobReplyId)
        {
            string[] includes = { $"{nameof(Job.JobComments)}.{nameof(JobComment.JobReplies)}" };
            Job item = await _getByIdAsync(jobId, true, includes);


            if (item.JobComments.FirstOrDefault(c => c.Id == jobCommentId) == null)
                throw new NotFoundException($"Comment with ID {jobCommentId} not found!");

            JobReply replyToRemove = item.JobComments.FirstOrDefault(c => c.Id == jobCommentId).JobReplies.FirstOrDefault(r => r.Id == jobReplyId);
            if (replyToRemove == null)
                throw new NotFoundException($"Reply with ID {jobReplyId} not found!");

            item.JobComments.FirstOrDefault(c => c.Id == jobCommentId).JobReplies.Remove(replyToRemove);

            _repository.Update(item);
            await _repository.SaveChangeAsync();

            return new($"Reply with ID {jobReplyId} has been deleted from comment with ID {jobCommentId} in job with ID {jobId}.");
        }

        private async Task<Job> _getByIdAsync(string id, bool isTracking = true, params string[] includes)
        {
            Job item = await _repository.GetByIdAsync(id, isTracking, includes);
            if (item == null)
                throw new NotFoundException($"Job not found({id})!");

            return item;
        }
    }
}
