using Workwise.Application.Dtos.Token;
using Workwise.Application.Dtos;
using Workwise.Application.Dtos.Accounts;

namespace Workwise.Application.Abstractions.Services
{
    public interface IAccountService
    {
        Task<ResultDto> RegisterAsync(RegisterDto register);
        Task<TokenResponseDto> LogInAsync(LoginDto login);
        Task<TokenResponseDto> LogInByRefreshToken(string refresh);
        Task<ResultDto> UpdateUserAsync(AppUserUpdateDto dto);
        Task<TokenResponseDto> ConfirmEmailAsync(ConfirmEmailDto dto);
        Task<TokenResponseDto> ChangePasswordAsync(ChangePasswordDto dto);
        Task<AppUserGetDto> GetCurrentUserAsync();
        Task<ResultDto> SendForgetPasswordMail(ForgetPasswordDto dto);
        Task<TokenResponseDto> ResetPasswordAsync(ResetPasswordTokenDto dto);
        Task<TokenResponseDto> ChangeEmailAsync(ChangeEmailDto dto);
        Task<string> GetUserRoleAsync(string AppUserId);
        Task<ResultDto> ChangeUserRoleAsync(ChangeRoleDto dto);
        Task<AppUserDto> GetUserByIdAsync(string id);
        Task<AppUserDto> GetUserByUsernameAsync(string userName);
        Task<PaginationDto<AppUserDto>> GetAllUsersFilteredAsync(string? search, int take, int page, int order, bool isActivate = false);
        Task<AppUserGetDto> CheckResetPasswordToken(ForgetPasswordTokenDto dto);
        Task<ResultDto> ChangePasswordByAdminAsync(ChangePasswordByAdminDto dto);
        Task<ResultDto> FollowUserAsync(string followingId);
        Task<ResultDto> UnFollowUserAsync(string followingId);
        Task<ResultDto> LikeJobAsync(string jobId);
        Task<ResultDto> UnLikeJobAsync(string jobId);
        Task<ResultDto> LikeProjectAsync(string projectId);
        Task<ResultDto> UnLikeProjectAsync(string projectId);
    }
}
