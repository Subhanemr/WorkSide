using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workwise.Application.Abstractions.Services;
using Workwise.Application.Dtos;
using Workwise.Application.Dtos.Messages;
using Workwise.Application.Dtos.Notifications;

namespace Workwise.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _service;

        public NotificationsController(INotificationService service)
        {
            _service = service;
        }

        [HttpGet("[Action]")]
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
        [HttpPost("[Action]")]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] NotificationCreateDto dto)
        {
            return Ok(await _service.CreateAsync(dto));
        }
        [HttpDelete("[Action]/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            return Ok(await _service.DeleteAsync(id));
        }
        [HttpDelete("[Action]/{id}")]
        [Authorize]
        public async Task<IActionResult> SoftDelete(string id)
        {
            return Ok(await _service.SoftDeleteAsync(id));
        }
        [HttpDelete("[Action]/{id}")]
        [Authorize]
        public async Task<IActionResult> ReverseSoftDelete(string id)
        {
            return Ok(await _service.ReverseSoftDeleteAsync(id));
        }
        [HttpPatch("[Action]")]
        [Authorize]
        public async Task<IActionResult> ReadAll()
        {
            return Ok(await _service.ReadAllNotificationsAsync());
        }
        [HttpPatch("[Action]/{id}")]
        [Authorize]
        public async Task<IActionResult> Read(string id)
        {
            return Ok(await _service.ReadNotificationAsync(id));
        }
    }
}
