using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper _mapper;

        public ProjectService(IProjectRepository repository)
        {
            _repository = repository;
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

            _repository.SoftDelete(item);
            await _repository.SaveChangeAsync();

            return new($"{item.Name} Project has been successfully soft deleted");
        }

        public async Task<ResultDto> ReverseSoftDeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new WrongRequestException("The provided id is null or empty");
            Project item = await _getByIdAsync(id);

            _repository.ReverseSoftDelete(item);
            await _repository.SaveChangeAsync();

            return new($"{item.Name} Project has been successfully restored");
        }

        public async Task<ResultDto> DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new WrongRequestException("The provided id is null or empty");
            Project item = await _getByIdAsync(id);

            _repository.Delete(item);
            await _repository.SaveChangeAsync();

            return new($"{item.Name} Project has been permanently deleted");
        }

        public async Task<PaginationDto<ProjectItemDto>> GetFilteredAsync(string? search, int take, int page, int order, bool isDeleted = false)
        {
            if (page <= 0) throw new WrongRequestException("The request sent does not exist");
            if (order <= 0) throw new WrongRequestException("The request sent does not exist");

            string[] includes = 
                { $"{nameof(Project.Category)}", $"{nameof(Project.AppUser)}", $"{nameof(Project.ProjectLikes)}", $"{nameof(Project.ProjectComments)}.{nameof(ProjectComment.ProjectReplies)}" };
            double count = await _repository
                .CountAsync(x => !string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true, false);

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

            PaginationDto<ProjectItemDto> pagination = new PaginationDto<ProjectItemDto>
            {
                Take = take,
                Search = search,
                Order = order,
                CurrentPage = page,
                TotalPage = Math.Ceiling(count / take),
                Items = dtos
            };

            return pagination;
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

        private async Task<Project> _getByIdAsync(string id, bool isTracking = true, params string[] includes)
        {
            Project item = await _repository.GetByIdAsync(id, isTracking, includes);
            if (item is null)
                throw new NotFoundException($"Project not found({id})!");

            return item;
        }
    }
}
