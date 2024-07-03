using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Workwise.Application.Abstractions.Services;
using Workwise.Application.Dtos;

namespace Workwise.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsService _service;

        public SettingsController(ISettingsService service)
        {
            _service = service;
        }

        [HttpGet("[Action]")]
        public async Task<IActionResult> Get(int page, int take, bool isDeleted = false)
        {
            return Ok();
        }
        [HttpGet("[Action]/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok();
        }
        [HttpPut("[Action]/{id}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Update(int id, [FromForm] CategoryUpdateDto update)
        {
            return NoContent();
        }
    }
}
