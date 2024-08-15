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
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _repository;
        private readonly IHttpContextAccessor _http;
        private readonly IMapper _mapper;

        public ProjectService(IProjectRepository repository, IHttpContextAccessor http)
        {
            _repository = repository;
            _http = http;
        }

        public async Task<ResultDto> CreateAsync(ProjectCreateDto dto)
        {
            if (await _repository.CheckUniqueAsync(x => x.Name == dto.Name))
                throw new AlreadyExistException($"{dto.Name} - This Project is already exist!");

            Project item = _mapper.Map<Project>(dto);

            await _repository.AddAsync(item);
            await _repository.SaveChangeAsync();

            return new($"{dto.Name} - Project is successfully created");
        }

        public async Task<ResultDto> SoftDeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new WrongRequestException("The provided id is null or empty");

            Project item = await _getByIdAsync(id);

            string currentUserId = _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isAdmin = _http.HttpContext.User.IsInRole("Admin");
            bool isModerator = _http.HttpContext.User.IsInRole("Moderator");
            if (item.AppUserId != currentUserId || !isAdmin || !isModerator)
                throw new WrongRequestException("You do not have permission to restore this job.");

            _repository.SoftDelete(item);
            await _repository.SaveChangeAsync();

            return new($"{item.Name} Project has been successfully soft deleted");
        }

        public async Task<ResultDto> ReverseSoftDeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new WrongRequestException("The provided id is null or empty");

            Project item = await _getByIdAsync(id);

            string currentUserId = _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isAdmin = _http.HttpContext.User.IsInRole("Admin");
            bool isModerator = _http.HttpContext.User.IsInRole("Moderator");
            if (item.AppUserId != currentUserId || !isAdmin || !isModerator)
                throw new WrongRequestException("You do not have permission to restore this job.");

            _repository.ReverseSoftDelete(item);
            await _repository.SaveChangeAsync();

            return new($"{item.Name} Project has been successfully restored");
        }

        public async Task<ResultDto> DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new WrongRequestException("The provided id is null or empty");

            string[] includes = { $"{nameof(Project.ProjectLikes)}", $"{nameof(Project.ProjectComments)}.{nameof(ProjectComment.ProjectReplies)}" };
            Project item = await _getByIdAsync(id, true, includes);

            _repository.Delete(item);
            await _repository.SaveChangeAsync();

            return new($"{item.Name} Project has been permanently deleted");
        }

        public async Task<PaginationDto<ProjectItemDto>> GetAllFilteredAsync(string? search, int take, int page, int order, bool isDeleted = false)
        {
            if (page <= 0)
                throw new WrongRequestException("Invalid page number.");
            if (take <= 0)
                throw new WrongRequestException("Invalid take value.");
            if (order <= 0)
                throw new WrongRequestException("Invalid order value.");

            string[] includes = 
                { $"{nameof(Project.Category)}", $"{nameof(Project.AppUser)}", $"{nameof(Project.ProjectLikes)}", $"{nameof(Project.ProjectComments)}.{nameof(ProjectComment.ProjectReplies)}" };
            double count = await _repository
                .CountAsync(x => !string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true, isDeleted);

            ICollection<Project> items = new List<Project>();

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

            ICollection<ProjectItemDto> dtos = _mapper.Map<ICollection<ProjectItemDto>>(items);

            return new ()
            {
                Take = take,
                Search = search,
                Order = order,
                CurrentPage = page,
                TotalPage = Math.Ceiling(count / take),
                Items = dtos
            };
        }

        public async Task<ProjectGetDto> GetByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new WrongRequestException("The provided id is null or empty");
            string[] includes =
                { $"{nameof(Project.Category)}", $"{nameof(Project.AppUser)}", $"{nameof(Project.ProjectLikes)}", $"{nameof(Project.ProjectComments)}.{nameof(ProjectComment.ProjectReplies)}" };
            Project item = await _getByIdAsync(id, false, includes);

            ProjectGetDto dto = _mapper.Map<ProjectGetDto>(item);
            return dto;
        }

        public async Task<ResultDto> UpdateAsync(ProjectUpdateDto dto)
        {
            Project existedItem = await _getByIdAsync(dto.Id);

            if (await _repository.CheckUniqueAsync(x => x.Name.ToLower() == dto.Name.ToLower().Trim() && x.Id != dto.Id))
                throw new AlreadyExistException($"{dto.Name}-This Project is already exist!");

            existedItem = _mapper.Map(dto, existedItem);
            _repository.Update(existedItem);
            await _repository.SaveChangeAsync();

            return new($"{dto.Name} - Project is successfully updated");
        }

        public async Task<ResultDto> AddCommentAsync(AddProjectCommentDto dto)
        {
            string[] includes = { $"{nameof(Project.ProjectComments)}.{nameof(ProjectComment.ProjectReplies)}" };
            Project item = await _getByIdAsync(dto.ProjectId, true, includes);

            ProjectComment comment = _mapper.Map<ProjectComment>(dto);
            comment.AppUserId = _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            item.ProjectComments.Add(comment);

            _repository.Update(item);
            await _repository.SaveChangeAsync();

            return new($"Comment added to job with ID {dto.ProjectId}.");
        }

        public async Task<ResultDto> DeleteCommentAsync(string projectId, string projectCommentId)
        {
            string[] includes = { $"{nameof(Project.ProjectComments)}.{nameof(ProjectComment.ProjectReplies)}" };
            Project item = await _getByIdAsync(projectId, true, includes);

            ProjectComment? commentToRemove = item.ProjectComments.FirstOrDefault(x => x.Id == projectCommentId);
            if (commentToRemove == null)
                throw new NotFoundException($"Comment with ID {projectCommentId} not found in job {projectId}.");

            item.ProjectComments.Remove(commentToRemove);

            _repository.Update(item);
            await _repository.SaveChangeAsync();

            return new($"Comment with ID {projectCommentId} has been removed from job {projectId}.");
        }

        public async Task<ResultDto> AddReplyAsync(AddProjectReplyDto dto)
        {
            string[] includes = { $"{nameof(Project.ProjectComments)}.{nameof(ProjectComment.ProjectReplies)}" };
            Project item = await _getByIdAsync(dto.ProjectId, true, includes);

            ProjectReply reply = _mapper.Map<ProjectReply>(dto);
            reply.AppUserId = _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (item.ProjectComments.FirstOrDefault(c => c.Id == dto.ProjectCommentId) == null)
                throw new NotFoundException($"Comment with ID {dto.ProjectCommentId} not found!");

            item.ProjectComments.FirstOrDefault(c => c.Id == dto.ProjectCommentId).ProjectReplies.Add(reply);

            _repository.Update(item);
            await _repository.SaveChangeAsync();

            return new($"Reply added to comment with ID {dto.ProjectCommentId} in job with ID {dto.ProjectId}.");
        }

        public async Task<ResultDto> DeleteReplyAsync(string projectId, string projectCommentId, string projectReplyId)
        {
            string[] includes = { $"{nameof(Project.ProjectComments)}.{nameof(ProjectComment.ProjectReplies)}" };
            Project item = await _getByIdAsync(projectId, true, includes);


            if (item.ProjectComments.FirstOrDefault(c => c.Id == projectCommentId) == null)
                throw new NotFoundException($"Comment with ID {projectCommentId} not found!");

            ProjectReply replyToRemove = item.ProjectComments.FirstOrDefault(c => c.Id == projectCommentId).ProjectReplies.FirstOrDefault(r => r.Id == projectReplyId);
            if (replyToRemove == null)
                throw new NotFoundException($"Reply with ID {projectReplyId} not found!");

            item.ProjectComments.FirstOrDefault(c => c.Id == projectCommentId).ProjectReplies.Remove(replyToRemove);

            _repository.Update(item);
            await _repository.SaveChangeAsync();

            return new($"Reply with ID {projectReplyId} has been deleted from comment with ID {projectCommentId} in job with ID {projectId}.");
        }

        private async Task<Project> _getByIdAsync(string id, bool isTracking = true, params string[] includes)
        {
            Project item = await _repository.GetByIdAsync(id, isTracking, includes);
            if (item is null)
                throw new NotFoundException($"Project not found({id})!");

            return item;
        }
    }
}
