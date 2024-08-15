using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Workwise.Application.Abstractions.Repositories;
using Workwise.Application.Abstractions.Services;
using Workwise.Application.Dtos;
using Workwise.Domain.Entities;
using Workwise.Persistance.Utilities;

namespace Workwise.Persistance.Implementations.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ResultDto> CreateAsync(CategoryCreateDto dto)
        {
            if (await _repository.CheckUniqueAsync(x => x.Name == dto.Name))
                throw new AlreadyExistException($"{dto.Name} - This Category is already exist!");
            Category item = _mapper.Map<Category>(dto);

            await _repository.AddAsync(item);
            await _repository.SaveChangeAsync();

            return new($"{dto.Name} - Category is successfully created");
        }

        public async Task<ResultDto> SoftDeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new WrongRequestException("The provided id is null or empty");
            Category item = await _getByIdAsync(id);

            _repository.SoftDelete(item);
            await _repository.SaveChangeAsync();

            return new($"{item.Name} category has been successfully soft deleted");
        }

        public async Task<ResultDto> ReverseSoftDeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new WrongRequestException("The provided id is null or empty");

            Category item = await _getByIdAsync(id);

            _repository.ReverseSoftDelete(item);
            await _repository.SaveChangeAsync();

            return new($"{item.Name} category has been successfully restored");
        }

        public async Task<ResultDto> DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new WrongRequestException("The provided id is null or empty");

            string[] includes = { $"{nameof(Category.Jobs)}", $"{nameof(Category.Projects)}" };
            Category item = await _getByIdAsync(id, true, includes);

            _repository.Delete(item);
            await _repository.SaveChangeAsync();

            return new($"{item.Name} category has been permanently deleted");
        }

        public async Task<PaginationDto<CategoryItemDto>> GetAllFilteredAsync(string? search, int take, int page, int order, bool isDeleted = false)
        {
            if (page <= 0)
                throw new WrongRequestException("Invalid page number.");
            if (take <= 0)
                throw new WrongRequestException("Invalid take value.");
            if (order <= 0)
                throw new WrongRequestException("Invalid order value.");

            string[] includes = { $"{nameof(Category.Jobs)}", $"{nameof(Category.Projects)}" };
            double count = await _repository
                .CountAsync(x => !string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true, isDeleted);

            ICollection<Category> items = new List<Category>();

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

            ICollection<CategoryItemDto> dtos = _mapper.Map<ICollection<CategoryItemDto>>(items);

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

        public async Task<CategoryGetDto> GetByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new WrongRequestException("The provided id is null or empty");
            string[] includes = { $"{nameof(Category.Jobs)}", $"{nameof(Category.Projects)}" };
            Category item = await _getByIdAsync(id, false, includes);

            CategoryGetDto dto = _mapper.Map<CategoryGetDto>(item);
            return dto;
        }

        public async Task<ResultDto> UpdateAsync(CategoryUpdateDto dto)
        {
            Category existedItem = await _getByIdAsync(dto.Id);

            if (await _repository.CheckUniqueAsync(x => x.Name.ToLower() == dto.Name.ToLower().Trim() && x.Id != dto.Id))
                throw new AlreadyExistException($"{dto.Name}-This Category is already exist!");

            existedItem = _mapper.Map(dto, existedItem);
            _repository.Update(existedItem);
            await _repository.SaveChangeAsync();

            return new($"{dto.Name} - Category is successfully updated");
        }

        private async Task<Category> _getByIdAsync(string id, bool isTracking = true, params string[] includes)
        {
            Category item = await _repository.GetByIdAsync(id, isTracking, includes);
            if (item is null)
                throw new NotFoundException($"Category not found({id})!");

            return item;
        }
    }
}
