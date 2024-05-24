using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Workwise.Application.Dtos.Token;
using Workwise.Domain.Entities;

namespace Workwise.Infrastructure.Infrastructure
{
    public class TokenHandler
    {
        private readonly IConfiguration _configuration;

        public TokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TokenResponseDto CreateJwt(AppUser user, ICollection<Claim> claims, int minutes)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecurityKey"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.Now.AddMinutes(minutes),
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return new TokenResponseDto(tokenString, tokenOptions.ValidTo, user.Name, CreateRefreshToken(), tokenOptions.ValidTo.AddMinutes(minutes / 4));
        }

        public string CreateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
