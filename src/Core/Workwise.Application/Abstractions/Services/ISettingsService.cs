using Workwise.Application.Dtos;
using Workwise.Application.Dtos.Settings;

namespace Workwise.Application.Abstractions.Services
{
    public interface ISettingsService
    {
        Task<PaginationDto<CategoryItemDto>> GetFilteredAsync(string? search, int take, int page, int order, bool isDeleted = false);
        Task<CategoryGetDto> GetByIdAsync(string id);
        Task<ResultDto> UpdateAsync(SettingUpdateDto dto);
    }
}
