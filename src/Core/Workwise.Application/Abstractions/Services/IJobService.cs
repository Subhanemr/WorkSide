using Workwise.Application.Dtos;

namespace Workwise.Application.Abstractions.Services
{
    public interface IJobService
    {
        Task<ResultDto> CreateAsync(JobCreateDto dto);
        Task<ResultDto> SoftDeleteAsync(string id);
        Task<ResultDto> ReverseSoftDeleteAsync(string id);
        Task<ResultDto> DeleteAsync(string id);
        Task<PaginationDto<JobItemDto>> GetAllFilteredAsync(string? search, int take, int page, int order, bool isDeleted = false);
        Task<JobGetDto> GetByIdAsync(string id);
        Task<ResultDto> UpdateAsync(JobUpdateDto dto);
        Task<ResultDto> AddCommentAsync(AddJobCommentDto dto);
        Task<ResultDto> DeleteCommentAsync(string jobId, string jobCommentId);
        Task<ResultDto> AddReplyAsync(AddJobReplyDto dto);
        Task<ResultDto> DeleteReplyAsync(string jobId, string jobCommentId, string jobReplyId);
    }
}
