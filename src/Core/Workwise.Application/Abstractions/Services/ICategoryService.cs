using Workwise.Application.Dtos;

namespace Workwise.Application.Abstractions.Services
{
    public interface ICategoryService
    {
        Task<ResultDto> CreateAsync(CategoryCreateDto dto);
        Task<ResultDto> SoftDeleteAsync(string id);
        Task<ResultDto> ReverseSoftDeleteAsync(string id);
        Task<ResultDto> DeleteAsync(string id);
        Task<PaginationDto<CategoryItemDto>> GetAllFilteredAsync(string? search, int take, int page, int order, bool isDeleted = false);
        Task<CategoryGetDto> GetByIdAsync(string id);
        Task<ResultDto> UpdateAsync(CategoryUpdateDto dto);
    }
}
