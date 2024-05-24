using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Workwise.Application.Abstractions.Services;

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

        [HttpGet("[Action]")]
        public async Task<IActionResult> Get(int page, int take, bool isDeleted = false)
        {
            return Ok();
        }
    }
}
