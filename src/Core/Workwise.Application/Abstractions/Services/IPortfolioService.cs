using Workwise.Application.Dtos;

namespace Workwise.Application.Abstractions.Services
{
    public interface IPortfolioService
    {
        Task<ResultDto> CreateAsync(PortfolioCreateDto dto);
        Task<ResultDto> SoftDeleteAsync(string id);
        Task<ResultDto> ReverseSoftDeleteAsync(string id);
        Task<ResultDto> DeleteAsync(string id);
        Task<PaginationDto<PortfolioItemDto>> GetFilteredAsync(string? search, int take, int page, int order, bool isDeleted = false);
        Task<PortfolioGetDto> GetByIdAsync(string id);
        Task<ResultDto> UpdateAsync(PortfolioUpdateDto dto);
    }
}
