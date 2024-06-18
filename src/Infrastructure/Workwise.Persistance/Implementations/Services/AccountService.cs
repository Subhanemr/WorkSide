using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;
using Workwise.Application.Abstractions.Services;
using Workwise.Application.Dtos;
using Workwise.Application.Dtos.Accounts;
using Workwise.Application.Dtos.Token;
using Workwise.Domain.Entities;
using Workwise.Domain.Enums;
using Workwise.Persistance.Utilities;

namespace Workwise.Persistance.Implementations.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly ITokenHandler _tokenHandler;
        private readonly IHttpContextAccessor _http;

        public AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            IMapper mapper, IEmailService emailService, IHttpContextAccessor http, ITokenHandler tokenHandler,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _emailService = emailService;
            _http = http;
            _tokenHandler = tokenHandler;
            _configuration = configuration;
        }

        public async Task<TokenResponseDto> LogInAsync(LoginDto login)
        {
            AppUser user = await _userManager.FindByNameAsync(login.UserNameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(login.UserNameOrEmail);
                if (user == null) throw new LoginException();
            }
            if (!user.EmailConfirmed) throw new LoginException("User is email not confirmed please check your email inbox");

            SignInResult result = await _signInManager.PasswordSignInAsync(user, login.Password, login.IsRemembered, true);
            if (result.IsLockedOut)
                throw new LoginException("The user is blocked and try again after 5 minutes");
            if (!result.Succeeded)
                throw new LoginException();

            ICollection<Claim> claims = await _userClaims(user);

            var tokenResponseDto = _tokenHandler.CreateJwt(user, claims, 60);
            user.RefreshToken = tokenResponseDto.RefreshToken;
            user.RefreshTokenExpireAt = tokenResponseDto.RefreshTokenExpireAt;
            await _userManager.UpdateAsync(user);

            return tokenResponseDto;
        }

        public async Task<ResultDto> RegisterAsync(RegisterDto register)
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


            ICollection<Claim> claims = await _userClaims(user);

            await _userManager.AddClaimsAsync(user, claims);

            TokenResponseDto token = _tokenHandler.CreateJwt(user, claims, 60);
            user.RefreshToken = token.RefreshToken;
            user.RefreshTokenExpireAt = token.RefreshTokenExpireAt;
            await _userManager.UpdateAsync(user);

            await SendEmailConfirmRequest(user);

            return new($"The user has been successfully created, please check your email inbox for email confirmation.");
        }

        public async Task<TokenResponseDto> LogInByRefreshToken(string refresh)
        {
            AppUser? user = await _userManager.Users.FirstOrDefaultAsync(x => x.RefreshToken == refresh);
            if (user == null)
                throw new NotFoundException("User is not found!");
            if (user.RefreshTokenExpireAt < DateTime.UtcNow)
                throw new LoginException("Token is expired at");

            TokenResponseDto tokenResponse = _tokenHandler.CreateJwt(user, await _userClaims(user), 60);
            user.RefreshToken = tokenResponse.RefreshToken;
            user.RefreshTokenExpireAt = tokenResponse.RefreshTokenExpireAt;
            await _userManager.UpdateAsync(user);

            return tokenResponse;
        }

        public async Task<string> GetUserRoleAsync(string userId)
        {
            AppUser user = await _getUserById(userId);
            var roles = await _userManager.GetRolesAsync(user);
            return roles.FirstOrDefault() ?? "null";
        }

        public async Task<TokenResponseDto> ChangePasswordAsync(ChangePasswordDto dto)
        {
            AppUser user = await _userManager.FindByIdAsync(_http.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (user is null)
                throw new NotFoundException("User is not found!");

            IdentityResult result = await _userManager.ChangePasswordAsync(user, dto.ExistPassword, dto.NewPassword);
            if (!result.Succeeded)
                throw new InvalidInputException(string.Join(" ", result.Errors.Select(e => e.Description)));

            return await _createAccesToken(user);
        }

        public async Task<TokenResponseDto> ConfirmEmailAsync(ConfirmEmailDto dto)
        {
            AppUser user = await _userManager.FindByIdAsync(dto.AppUserId);
            if (user is null)
                throw new NotFoundException("User is not found!");

            IdentityResult result = await _userManager.ConfirmEmailAsync(user, dto.Token);
            if (!result.Succeeded)
                throw new InvalidInputException(string.Join(" ", result.Errors.Select(e => e.Description)));

            return await _createAccesToken(user);
        }

        public async Task<TokenResponseDto> ChangeEmailAsync(ChangeEmailDto dto)
        {
            AppUser user = await _getUserById(dto.Id);
            AppUserGetDto currentUser = await GetCurrentUserAsync();
            if (user.Id != currentUser.Id)
                throw new UnAuthorizedException();

            IdentityResult result = await _userManager.ChangeEmailAsync(user, dto.Email, dto.Token);
            if (!result.Succeeded)
                throw new InvalidInputException(string.Join(" ", result.Errors.Select(e => e.Description)));

            return await _createAccesToken(user);
        }

        public async Task<AppUserDto> GetUserByIdAsync(string id)
        {
            AppUser? user = await _userManager.Users
                .Include(x => x.Follows)
                .Include(x => x.Followings)
                .Include(x => x.Skills)
                .Include(x => x.Educations)
                .Include(x => x.Experiences)
                .Include(x => x.Portfolios)
                .Include(x => x.Locations)
                .Include(x => x.HireAccounts)
                .Include(x => x.Jobs)
                .Include(x => x.Projects).FirstOrDefaultAsync(x => x.Id == id);


            if (user is null)
                throw new NotFoundException("User is not found!");

            AppUserDto dto = _mapper.Map<AppUserDto>(user);

            return dto;
        }

        public async Task<PaginationDto<AppUserDto>> GetUsersAsync(string? search, int take, int page, int order)
        {
            if (page <= 0) throw new WrongRequestException("The request sent does not exist");
            if (order <= 0) throw new WrongRequestException("The request sent does not exist");

            double count = await _userManager.Users.Where(x => !string.IsNullOrEmpty(search) ? x.UserName.ToLower().Contains(search.ToLower()) : true)
                        .Where(x => x.UserName != _configuration["AdminSettings:UserName"] && x.UserName != _configuration["ModeratorSettings:UserName"])
                        .Where(x => x.IsActivate == false).CountAsync();


            ICollection<AppUser> users = new List<AppUser>();

            switch (order)
            {
                case 1:
                    users = await _userManager.Users.Where(x => !string.IsNullOrEmpty(search) ? x.UserName.ToLower().Contains(search.ToLower()) : true)
                        .Where(x => x.UserName != _configuration["AdminSettings:UserName"] && x.UserName != _configuration["ModeratorSettings:UserName"])
                        .Where(x => x.IsActivate == false).OrderBy(x => x.UserName).Skip((page - 1) * take).Take(take).AsNoTracking().ToListAsync();
                    break;
                case 2:
                    users = await _userManager.Users.Where(x => !string.IsNullOrEmpty(search) ? x.UserName.ToLower().Contains(search.ToLower()) : true)
                        .Where(x => x.UserName != _configuration["AdminSettings:UserName"] && x.UserName != _configuration["ModeratorSettings:UserName"])
                        .Where(x => x.IsActivate == false).OrderByDescending(x => x.UserName).Skip((page - 1) * take).Take(take).AsNoTracking().ToListAsync();
                    break;
                case 3:
                    users = await _userManager.Users.Where(x => !string.IsNullOrEmpty(search) ? x.UserName.ToLower().Contains(search.ToLower()) : true)
                        .Where(x => x.UserName != _configuration["AdminSettings:UserName"] && x.UserName != _configuration["ModeratorSettings:UserName"])
                        .Where(x => x.IsActivate == false).OrderBy(x => x.Name).Skip((page - 1) * take).Take(take).AsNoTracking().ToListAsync();
                    break;
                case 4:
                    users = await _userManager.Users.Where(x => !string.IsNullOrEmpty(search) ? x.UserName.ToLower().Contains(search.ToLower()) : true)
                        .Where(x => x.UserName != _configuration["AdminSettings:UserName"] && x.UserName != _configuration["ModeratorSettings:UserName"])
                        .Where(x => x.IsActivate == false).OrderByDescending(x => x.Name).Skip((page - 1) * take).Take(take).AsNoTracking().ToListAsync();
                    break;
            }

            ICollection<AppUserDto> vMs = _mapper.Map<ICollection<AppUserDto>>(users);

            PaginationDto<AppUserDto> pagination = new PaginationDto<AppUserDto>
            {
                Take = take,
                Search = search,
                Order = order,
                CurrentPage = page,
                TotalPage = Math.Ceiling(count / take),
                Items = vMs
            };

            return pagination;
        }

        public async Task<PaginationDto<AppUserDto>> GetDeletedUsersAsync(string? search, int take, int page, int order)
        {
            if (page <= 0) throw new WrongRequestException("The request sent does not exist");
            if (order <= 0) throw new WrongRequestException("The request sent does not exist");

            double count = await _userManager.Users.Where(x => !string.IsNullOrEmpty(search) ? x.UserName.ToLower().Contains(search.ToLower()) : true)
                        .Where(x => x.UserName != _configuration["AdminSettings:UserName"] && x.UserName != _configuration["ModeratorSettings:UserName"])
                        .Where(x => x.IsActivate == true).CountAsync();

            ICollection<AppUser> users = new List<AppUser>();

            switch (order)
            {
                case 1:
                    users = await _userManager.Users.Where(x => !string.IsNullOrEmpty(search) ? x.UserName.ToLower().Contains(search.ToLower()) : true)
                        .Where(x => x.UserName != _configuration["AdminSettings:UserName"] && x.UserName != _configuration["ModeratorSettings:UserName"])
                        .Where(x => x.IsActivate == true).OrderBy(x => x.UserName).Skip((page - 1) * take).Take(take).AsNoTracking().ToListAsync();
                    break;
                case 2:
                    users = await _userManager.Users.Where(x => !string.IsNullOrEmpty(search) ? x.UserName.ToLower().Contains(search.ToLower()) : true)
                        .Where(x => x.UserName != _configuration["AdminSettings:UserName"] && x.UserName != _configuration["ModeratorSettings:UserName"])
                        .Where(x => x.IsActivate == true).OrderByDescending(x => x.UserName).Skip((page - 1) * take).Take(take).AsNoTracking().ToListAsync();
                    break;
                case 3:
                    users = await _userManager.Users.Where(x => !string.IsNullOrEmpty(search) ? x.UserName.ToLower().Contains(search.ToLower()) : true)
                        .Where(x => x.UserName != _configuration["AdminSettings:UserName"] && x.UserName != _configuration["ModeratorSettings:UserName"])
                        .Where(x => x.IsActivate == true).OrderBy(x => x.Name).Skip((page - 1) * take).Take(take).AsNoTracking().ToListAsync();
                    break;
                case 4:
                    users = await _userManager.Users.Where(x => !string.IsNullOrEmpty(search) ? x.UserName.ToLower().Contains(search.ToLower()) : true)
                        .Where(x => x.UserName != _configuration["AdminSettings:UserName"] && x.UserName != _configuration["ModeratorSettings:UserName"])
                        .Where(x => x.IsActivate == true).OrderByDescending(x => x.Name).Skip((page - 1) * take).Take(take).AsNoTracking().ToListAsync();
                    break;
            }

            ICollection<AppUserDto> vMs = _mapper.Map<ICollection<AppUserDto>>(users);

            PaginationDto<AppUserDto> pagination = new PaginationDto<AppUserDto>
            {
                Take = take,
                Search = search,
                Order = order,
                CurrentPage = page,
                TotalPage = Math.Ceiling(count / take),
                Items = vMs
            };

            return pagination;
        }

        private async Task<AppUserGetDto> GetCurrentUserAsync()
        {
            string? id = _http.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id is null)
                throw new UnAuthorizedException();

            AppUser user = await _userManager.FindByIdAsync(id);
            if (user is null)
                throw new UnAuthorizedException();

            AppUserGetDto dto = _mapper.Map<AppUserGetDto>(user);

            dto.Role = await GetUserRoleAsync(dto.Id);

            return dto;
        }

        private async Task<AppUser> _getUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                throw new NotFoundException("This user is not found");
            return user;
        }

        private async Task SendEmailConfirmRequest(AppUser user)
        {
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            string path = Path.Combine("http://localhost:3000", $"ConfirmEmail?AppUserId={user.Id}&token={token}");
            string body = _confirmEmailBody.Replace("{Replace_Link_1}", path);
            body = body.Replace("{Replace_Link_2}", path);
            body = body.Replace("{Replace_Link_3}", path);


            await _emailService.SendMailAsync(user.Email, "WorkWise Email Confirm", path, true);
        }

        private async Task SendEmailChangeRequest(AppUser user, string email)
        {
            string token = await _userManager.GenerateChangeEmailTokenAsync(user, email); 
            string path = Path.Combine("http://localhost:3000", $"ChangeEmail?AppUserId={user.Id}&token={token}&email={email}");
            string body = _confirmEmailBody.Replace("{Replace_Link_1}", path);
            body = body.Replace("{Replace_Link_2}", path);
            body = body.Replace("{Replace_Link_3}", path);


            await _emailService.SendMailAsync(email, "WorkWise Change Email", body, true);
        }

        private async Task<TokenResponseDto> _createAccesToken(AppUser user)
        {
            ICollection<Claim> claims = (await _userManager.GetClaimsAsync(user)).ToList();
            TokenResponseDto token = _tokenHandler.CreateJwt(user, claims, 60);
            user.RefreshToken = token.RefreshToken;
            user.RefreshTokenExpireAt = token.RefreshTokenExpireAt;
            await _userManager.UpdateAsync(user);

            return token;
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

        private string _resetPasswordBody = "<!DOCTYPE html>\r\n<html lang=\"az\">\r\n<head>\r\n<meta charset=\"UTF-8\">\r\n<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n<title>Şifrəni Sıfırla</title>\r\n</head>\r\n<body style=\"font-family: 'Roboto', sans-serif; background-color: #f4f4f4; padding: 20px; font-weight: 600;\">\r\n\r\n<div style=\"max-width: 600px; margin: 0 auto; background-color: #fff; padding: 20px; border-radius: 10px; box-shadow: 0 0 10px rgba(0,0,0,0.1);\">\r\n    <h2 style=\"text-align: center; color: #D18337; font-weight: bold;\">Şifrəni Sıfırla</h2>\r\n    <p style=\"color: #555; text-align: justify;\">Hi,</p>\r\n    <p style=\"color: #555; text-align: justify;\">Click the button below to reset your account password:</p>\r\n    <div style=\"text-align: center; margin-top: 20px;\">\r\n        <a href=\"{Replace_Link_1}\" style=\"display: inline-block; background-color: #D18337; color: #fff; text-decoration: none; padding: 10px 20px; border-radius: 5px; font-weight: bold;\">Reset Password</a>\r\n    </div>\r\n    <p style=\"color: #555; text-align: justify; margin-top: 20px;\">If you're having trouble clicking the button above, paste the following link into your browser's address bar:</p>\r\n    <p style=\"color: #555; text-align: center;\"><a href=\"{Replace_Link_2}\" style=\"color: #D18337; text-decoration: none; font-weight: bold;\">{Replace_Link_3}</a></p>\r\n    <p style=\"color: #555; text-align: justify; margin-top: 20px;\">If you have not made this request, please ignore.</p>\r\n    <p style=\"color: #555; text-align: justify;\">Thanks!</p>\r\n    <p style=\"color: #D18337; text-align: justify;\">This email was sent by WorkWise.</p>\r\n</div>\r\n\r\n</body>\r\n</html>\r\n";
        private string _confirmEmailBody = "<!DOCTYPE html>\r\n<html lang=\"az\">\r\n<head>\r\n<meta charset=\"UTF-8\">\r\n<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n<title>Email Təsdiqi</title>\r\n</head>\r\n<body style=\"font-family: 'Roboto', sans-serif; background-color: #f4f4f4; padding: 20px; font-weight: 600;\">\r\n\r\n<div style=\"max-width: 600px; margin: 0 auto; background-color: #fff; padding: 20px; border-radius: 10px; box-shadow: 0 0 10px rgba(0,0,0,0.1);\">\r\n    <h2 style=\"text-align: center; color: #D18337; font-weight: bold;\">E-poçt Təsdiqi</h2>\r\n    <p style=\"color: #555; text-align: justify;\">Hi,</p>\r\n    <p style=\"color: #555; text-align: justify;\">Click the button below to confirm your email address:</p>\r\n    <div style=\"text-align: center; margin-top: 20px;\">\r\n        <a href=\"{Replace_Link_1}\" style=\"display: inline-block; background-color: #D18337; color: #fff; text-decoration: none; padding: 10px 20px; border-radius: 5px; font-weight: bold;\">Confirm My Email Address</a>\r\n    </div>\r\n    <p style=\"color: #555; text-align: justify; margin-top: 20px;\">If you're having trouble clicking the button above, paste the following link into your browser's address bar:</p>\r\n    <p style=\"color: #555; text-align: center;\"><a href=\"{Replace_Link_2}\" style=\"color: #D18337; text-decoration: none; font-weight: bold;\">{Replace_Link_3}</a></p>\r\n    <p style=\"color: #555; text-align: justify; margin-top: 20px;\">If you have not made this request, please ignore.</p>\r\n    <p style=\"color: #555; text-align: justify;\">Thanks!</p>\r\n    <p style=\"color: #D18337; text-align: justify;\">This email was sent by WorkWise.</p>\r\n</div>\r\n\r\n</body>\r\n</html>\r\n";
    }
}
