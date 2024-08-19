using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Workwise.Application.Abstractions.Services;

namespace Workwise.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatsController : ControllerBase
    {
        private readonly IChatService _service;

        public ChatsController(IChatService service)
        {
            _service = service;
        }

        [HttpGet("[Action]/{id}")]
        [Authorize]
        public async Task<IActionResult> GetAll(string? search, int take, int page, int order)
        {
            return Ok(await _service.GetAllFilteredAsync(search, take, page, order));
        }
        [HttpGet("[Action]")]
        [Authorize]
        public async Task<IActionResult> GetAllDeleted(string? search, int take, int page, int order)
        {
            return Ok(await _service.GetAllFilteredAsync(search, take, page, order, true));
        }
        [HttpGet("[Action]/{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }
        [HttpPost("[Action/{user2Id}]")]
        [Authorize]
        public async Task<IActionResult> Create(string user2Id)
        {
            return Ok( await _service.CreateAsync(user2Id));
        }
        [HttpDelete("[Action]/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            return Ok(await _service.DeleteAsync(id));
        }
        [HttpDelete("[Action/{id}]")]
        [Authorize]
        public async Task<IActionResult> SoftDelete(string id)
        {
            return Ok(await _service.SoftDeleteAsync(id));
        }
        [HttpDelete("[Action/{id}]")]
        [Authorize]
        public async Task<IActionResult> ReverseSoftDelete(string id)
        {
            return Ok(await _service.ReverseSoftDeleteAsync(id));
        }
    }
}
