using System.Security.Claims;
using Workwise.Application.Dtos.Token;
using Workwise.Domain.Entities;

namespace Workwise.Application.Abstractions.Services
{
    public interface ITokenHandler
    {
        TokenResponseDto CreateJwt(AppUser user, ICollection<Claim> claims, int minutes);
    }
}
