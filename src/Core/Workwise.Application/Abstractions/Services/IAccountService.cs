using Workwise.Application.Dtos.Token;
using Workwise.Application.Dtos;

namespace Workwise.Application.Abstractions.Services
{
    public interface IAccountService
    {
        Task RegisterAsync(RegisterDto register);
        Task<TokenResponseDto> LogInAsync(LoginDto login);

        Task<TokenResponseDto> LogInByRefreshToken(string refresh);
    }
}
