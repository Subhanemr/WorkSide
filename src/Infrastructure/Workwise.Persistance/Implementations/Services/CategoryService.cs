using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
            Category category = _mapper.Map<Category>(dto);

            await _repository.AddAsync(category);
            await _repository.SaveChangeAsync();

            return new($"{dto.Name} - Category is successfully created");
        }

        public async Task<ResultDto> SoftDeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new WrongRequestException("The provided id is null or empty");
            Category category = await _getCategoryByIdAsync(id);

            _repository.SoftDelete(category);
            await _repository.SaveChangeAsync();

            return new($"{category.Name} category has been successfully soft deleted");
        }

        public async Task<ResultDto> ReverseSoftDeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new WrongRequestException("The provided id is null or empty");
            Category category = await _getCategoryByIdAsync(id);

            _repository.ReverseSoftDelete(category);
            await _repository.SaveChangeAsync();

            return new($"{category.Name} category has been successfully restored");
        }

        public async Task<ResultDto> DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new WrongRequestException("The provided id is null or empty");
            Category category = await _getCategoryByIdAsync(id);

            _repository.Delete(category);
            await _repository.SaveChangeAsync();

            return new($"{category.Name} category has been permanently deleted");
        }

        public async Task<PaginationDto<CategoryItemDto>> GetFilteredAsync(string? search, int take, int page, int order, bool isDeleted = false)
        {
            if (page <= 0) throw new WrongRequestException("The request sent does not exist");
            if (order <= 0) throw new WrongRequestException("The request sent does not exist");

            string[] includes = { $"{nameof(Category.Jobs)}", $"{nameof(Category.Projects)}" };
            double count = await _repository
                .CountAsync(x => !string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true, false);

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
                        x => x.Name, true, isDeleted, (page - 1) * take, take: take, false, includes).ToListAsync();
                    break;
                case 4:
                    items = await _repository
                     .GetAllWhereByOrder(x => !string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true,
                      x => x.CreateAt, true, isDeleted, (page - 1) * take, take: take, IsTracking: false, includes: includes).ToListAsync();
                    break;
            }

            ICollection<CategoryItemDto> vMs = _mapper.Map<ICollection<CategoryItemDto>>(items);

            PaginationDto<CategoryItemDto> pagination = new PaginationDto<CategoryItemDto>
            {
                Take = take,
                Search = search,
                Order = order,
                CurrentPage = page,
                TotalPage = Math.Ceiling(count / take),
                Items = vMs
            };

            return pagination;
        }

        public async Task<CategoryGetDto> GetByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new WrongRequestException("The provided id is null or empty");
            string[] includes = { $"{nameof(Category.Jobs)}", $"{nameof(Category.Projects)}" };
            Category category = await _getCategoryByIdAsync(id, false, includes);

            var dto = _mapper.Map<CategoryGetDto>(category);
            return dto;
        }

        public async Task<ResultDto> UpdateAsync(CategoryUpdateDto dto)
        {
            Category existedCategory = await _getCategoryByIdAsync(dto.Id);

            if (await _repository.CheckUniqueAsync(x => x.Name.ToLower() == dto.Name.ToLower().Trim() && x.Id != dto.Id))
                throw new AlreadyExistException($"{dto.Name}-This Category is already exist!");

            existedCategory = _mapper.Map(dto, existedCategory);
            _repository.Update(existedCategory);
            await _repository.SaveChangeAsync();

            return new($"{dto.Name} - Category is successfully updated");
        }

        private async Task<Category> _getCategoryByIdAsync(string id, bool isTracking = true, params string[] includes)
        {
            Category category = await _repository.GetByIdAsync(id, isTracking, includes);
            if (category is null)
                throw new NotFoundException($"Category not found({id})!");

            return category;
        }
    }
}
