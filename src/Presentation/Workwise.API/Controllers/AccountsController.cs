﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workwise.Application.Abstractions.Services;
using Workwise.Application.Dtos;
using Workwise.Application.Dtos.Accounts;

namespace Workwise.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _service;

        public AccountsController(IAccountService service)
        {
            _service = service;
        }

        [HttpPost("[Action]")]
        public async Task<IActionResult> Register([FromForm] RegisterDto register)
        {
            await _service.RegisterAsync(register);
            return NoContent();
        }
        [HttpPost("[Action]")]
        public async Task<IActionResult> LogIn([FromForm] LoginDto login)
        {
            var result = await _service.LogInAsync(login);
            return Ok(result);
        }
        [HttpPost("[Action]")]
        public async Task<IActionResult> LogInByRefresh(string refToken)
        {
            return Ok(await _service.LogInByRefreshToken(refToken));
        }
        [HttpPut("[Action]")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(AppUserUpdateDto dto)
        {
            return Ok(await _service.UpdateUserAsync(dto));
        }
        [HttpGet("[Action]")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUserInfo()
        {
            return Ok(await _service.GetCurrentUserAsync());
        }
        [HttpPost("[Action]")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDto dto)
        {
            return Ok(await _service.SendForgetPasswordMail(dto));
        }
        [HttpPut("[Action]")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            return Ok(await _service.ChangePasswordAsync(dto));
        }
        [HttpPut("[Action]")]
        public async Task<IActionResult> ResetPassword(ResetPasswordTokenDto dto)
        {
            return Ok(await _service.ResetPasswordAsync(dto));
        }
        [HttpPatch("[Action]")]
        [Authorize]
        public async Task<IActionResult> ChangeEmail(ChangeEmailDto dto)
        {
            return Ok(await _service.ChangeEmailAsync(dto));
        }
        [HttpPatch("[Action]")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailDto dto)
        {
            return Ok(await _service.ConfirmEmailAsync(dto));
        }
        [HttpGet("[Action]/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            return Ok(await _service.GetUserByIdAsync(id));
        }
        [HttpGet("[Action]")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            return Ok(await _service.GetCurrentUserAsync());
        }
        [HttpPut("[Action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeUserRole(ChangeRoleDto dto)
        {
            return Ok(await _service.ChangeUserRoleAsync(dto));
        }
        [HttpPut("[Action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeUserPassword(ChangePasswordByAdminDto dto)
        {
            return Ok(await _service.ChangePasswordByAdminAsync(dto));
        }
        [HttpPost("[Action]")]
        public async Task<IActionResult> GetAllUsers(string? search, int take, int page, int order = 1)
        {
            return Ok(await _service.GetAllUsersFilteredAsync(search, take, page, order));
        }
        [HttpPost("[Action]")]
        public async Task<IActionResult> GetAllDeletedUsers(string? search, int take, int page, int order = 1)
        {
            return Ok(await _service.GetAllUsersFilteredAsync(search, take, page, order, true));
        }
        [HttpPatch("[Action]/{followingId}")]
        [Authorize]
        public async Task<IActionResult> FollowUser(string followingId)
        {
            return Ok(await _service.FollowUserAsync(followingId));
        }
        [HttpPatch("[Action]/{followingId}")]
        [Authorize]
        public async Task<IActionResult> UnFollowUser(string followingId)
        {
            return Ok(await _service.UnFollowUserAsync(followingId));
        }
        [HttpPatch("[Action]/{jobId}")]
        [Authorize]
        public async Task<IActionResult> LikeJob(string jobId)
        {
            return Ok(await _service.LikeJobAsync(jobId));
        }
        [HttpPatch("[Action]/{jobId}")]
        [Authorize]
        public async Task<IActionResult> UnLikeJob(string jobId)
        {
            return Ok(await _service.UnLikeJobAsync(jobId));
        }
        [HttpPatch("[Action]/{projectId}")]
        [Authorize]
        public async Task<IActionResult> LikeProject(string projectId)
        {
            return Ok(await _service.LikeProjectAsync(projectId));
        }
        [HttpPatch("[Action]/{projectId}")]
        [Authorize]
        public async Task<IActionResult> UnLikeProject(string projectId)
        {
            return Ok(await _service.UnLikeProjectAsync(projectId));
        }
        [HttpPost("[Action]/{id}")]
        [Authorize]
        public async Task<IActionResult> HireAccount(string id)
        {
            return Ok(await _service.HireAccountAsync(id));
        }
        [HttpDelete("[Action]/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteHireAccount(string id)
        {
            return Ok(await _service.DeleteHireAccountAsync(id));
        }
    }
}
