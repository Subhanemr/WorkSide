using Microsoft.AspNetCore.Mvc;
using Workwise.Application.Abstractions.Services;
using Workwise.Application.Dtos;

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
    }
}
