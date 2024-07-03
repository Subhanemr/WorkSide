using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workwise.Application.Abstractions.Services;
using Workwise.Application.Dtos;

namespace Workwise.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet("[Action]")]
        public async Task<IActionResult> Get(string? search, int take, int page, int order)
        {
            return Ok(await _service.GetFilteredAsync(search, take, page, order));
        }
        [HttpGet("[Action]")]
        public async Task<IActionResult> GetDeleted(string? search, int take, int page, int order)
        {
            return Ok(await _service.GetFilteredAsync(search, take, page, order, true));
        }
        [HttpGet("[Action]/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }
        [HttpPost("[Action]")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Create([FromForm] CategoryCreateDto dto)
        {
            return Ok(await _service.CreateAsync(dto));
        }
        [HttpPut("[Action]")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Update([FromForm] CategoryUpdateDto dto)
        {
            return Ok(await _service.UpdateAsync(dto));
        }
        [HttpDelete("[Action]/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            return Ok(await _service.DeleteAsync(id));
        }
        [HttpDelete("[Action]/{id}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> SoftDelete(string id)
        {
            return Ok(await _service.SoftDeleteAsync(id));
        }
        [HttpDelete("[Action]/{id}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> ReverseSoftDelete(string id)
        {
            return Ok(await _service.ReverseSoftDeleteAsync(id));
        }
    }
}
