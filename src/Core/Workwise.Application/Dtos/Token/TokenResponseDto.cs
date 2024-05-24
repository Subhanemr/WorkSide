namespace Workwise.Application.Dtos.Token
{
    public record TokenResponseDto(string Token, DateTime ExpireTime, string UserName, string RefreshToken, DateTime RefreshTokenExpire);
}
