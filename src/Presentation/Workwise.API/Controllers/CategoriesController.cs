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
        public async Task<IActionResult> Get(int page, int take, bool isDeleted = false)
        {
            return Ok();
        }
        [HttpGet("[Action]/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok();
        }
        [HttpPost("[Action]")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Create([FromForm] CategoryCreateDto create)
        {
            return StatusCode(StatusCodes.Status201Created);
        }
        [HttpPut("[Action]/{id}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Update(int id, [FromForm] CategoryUpdateDto update)
        {
            return NoContent();
        }
        [HttpDelete("[Action]/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            return NoContent();
        }
        [HttpDelete("[Action]/{id}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            return NoContent();
        }
        [HttpDelete("[Action]/{id}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> ReverseSoftDelete(int id)
        {
            return NoContent();
        }
    }
}
