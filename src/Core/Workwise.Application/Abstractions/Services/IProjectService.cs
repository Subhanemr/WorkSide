using Workwise.Application.Dtos;

namespace Workwise.Application.Abstractions.Services
{
    public interface IProjectService
    {
        Task<ResultDto> CreateAsync(ProjectCreateDto dto);
        Task<ResultDto> SoftDeleteAsync(string id);
        Task<ResultDto> ReverseSoftDeleteAsync(string id);
        Task<ResultDto> DeleteAsync(string id);
        Task<PaginationDto<ProjectItemDto>> GetFilteredAsync(string? search, int take, int page, int order, bool isDeleted = false);
        Task<ProjectGetDto> GetByIdAsync(string id);
        Task<ResultDto> UpdateAsync(ProjectUpdateDto dto);
    }
}
