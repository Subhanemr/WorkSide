using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using Workwise.Application.Abstractions.Services;
using Workwise.Application.Dtos;
using Workwise.Application.Dtos.Token;
using Workwise.Domain.Entities;
using Workwise.Domain.Enums;

namespace Workwise.Persistance.Implementations.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ITokenHandler _tokenHandler;
        private readonly IHttpContextAccessor _http;

        public AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            IMapper mapper, IEmailService emailService, IHttpContextAccessor http, ITokenHandler tokenHandler)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _emailService = emailService;
            _http = http;
            _tokenHandler = tokenHandler;
        }

        public async Task<TokenResponseDto> LogInAsync(LoginDto login)
        {
            AppUser user = await _userManager.FindByNameAsync(login.UserNameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(login.UserNameOrEmail);
                if (user == null) throw new Exception("Username, Email or Password is incorrect");
            }

            SignInResult result = await _signInManager.PasswordSignInAsync(user, login.Password, login.IsRemembered, true);
            if (result.IsLockedOut) throw new Exception("Login is not enable please try latter");
            if (!result.Succeeded) throw new Exception("Username, Email or Password is incorrect");

            ICollection<Claim> claims = await _userClaims(user);

            var tokenResponseDto = _tokenHandler.CreateJwt(user, claims, 60);
            user.RefreshToken = tokenResponseDto.RefreshToken;
            user.RefreshTokenExpireAt = tokenResponseDto.RefreshTokenExpire;
            await _userManager.UpdateAsync(user);

            return tokenResponseDto;
        }

        public async Task RegisterAsync(RegisterDto register)
        {
            AppUser user = _mapper.Map<AppUser>(register);

            IdentityResult result = await _userManager.CreateAsync(user, register.Password);
            StringBuilder message = new StringBuilder();
            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    message.AppendLine(error.Description);
                }
            }

            await _userManager.AddToRoleAsync(user, UserRoles.Member.ToString());
        }

        public async Task<TokenResponseDto> LogInByRefreshToken(string refresh)
        {
            AppUser? user = await _userManager.Users.FirstOrDefaultAsync(x => x.RefreshToken == refresh);
            if (user == null) throw new Exception("Not found");
            if (user.RefreshTokenExpireAt < DateTime.UtcNow) throw new Exception("Token is Expired");

            var tokenResponse = _tokenHandler.CreateJwt(user, await _userClaims(user), 60);
            user.RefreshToken = tokenResponse.RefreshToken;
            user.RefreshTokenExpireAt = tokenResponse.RefreshTokenExpire;
            await _userManager.UpdateAsync(user);

            return tokenResponse;

        }

        private async Task<ICollection<Claim>> _userClaims(AppUser user)
        {
            ICollection<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.Name),
                new Claim(ClaimTypes.Surname, user.Surname),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            foreach (var role in await _userManager.GetRolesAsync(user))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }
    }
}
