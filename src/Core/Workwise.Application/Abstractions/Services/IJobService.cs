using Workwise.Application.Dtos;

namespace Workwise.Application.Abstractions.Services
{
    public interface IJobService
    {
        Task<ResultDto> CreateAsync(JobCreateDto dto);
        Task<ResultDto> SoftDeleteAsync(string id);
        Task<ResultDto> ReverseSoftDeleteAsync(string id);
        Task<ResultDto> DeleteAsync(string id);
        Task<PaginationDto<JobItemDto>> GetFilteredAsync(string? search, int take, int page, int order, bool isDeleted = false);
        Task<JobGetDto> GetByIdAsync(string id);
        Task<ResultDto> UpdateAsync(JobUpdateDto dto);
    }
}
