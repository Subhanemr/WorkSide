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
        public async Task<IActionResult> Get(string? search, int take, int page, int order)
        {
            return Ok(await _service.GetAllFilteredAsync(search, take, page, order));
        }
        [HttpGet("[Action]/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }
        [HttpPut("[Action]")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Update([FromForm] SettingUpdateDto dto)
        {
            return Ok(await _service.UpdateAsync(dto));
        }
    }
}
