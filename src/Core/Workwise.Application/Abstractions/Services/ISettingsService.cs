using Workwise.Application.Dtos;

namespace Workwise.Application.Abstractions.Services
{
    public interface ISettingsService
    {
        Task<PaginationDto<CategoryItemDto>> GetFilteredAsync(string? search, int take, int page, int order);
        Task<CategoryGetDto> GetByIdAsync(string id);
        Task<ResultDto> UpdateAsync(SettingUpdateDto dto);
    }
}
