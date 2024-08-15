using Workwise.Application.Dtos;

namespace Workwise.Application.Abstractions.Services
{
    public interface ISettingsService
    {
        Task<PaginationDto<SettingsItemDto>> GetAllFilteredAsync(string? search, int take, int page, int order, bool isDeleted = false);
        Task<SettingsGetDto> GetByIdAsync(string id);
        Task<ResultDto> UpdateAsync(SettingUpdateDto dto);
    }
}
