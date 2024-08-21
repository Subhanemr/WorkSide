﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Workwise.Application.Abstractions.Services;
using Workwise.Application.Dtos;

namespace Workwise.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _service;

        public ProjectsController(IProjectService service)
        {
            _service = service;
        }
        [HttpGet("[Action]")]
        public async Task<IActionResult> GetAll(string? search, int take, int page, int order)
        {
            return Ok(await _service.GetAllFilteredAsync(search, take, page, order));
        }
        [HttpGet("[Action]")]
        public async Task<IActionResult> GetAllDeleted(string? search, int take, int page, int order)
        {
            return Ok(await _service.GetAllFilteredAsync(search, take, page, order, true));
        }
        [HttpGet("[Action]/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }
        [HttpPost("[Action]")]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] ProjectCreateDto dto)
        {
            return Ok(await _service.CreateAsync(dto));
        }
        [HttpPut("[Action]")]
        [Authorize]
        public async Task<IActionResult> Update([FromForm] ProjectUpdateDto dto)
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
        [HttpPost("[Action]")]
        [Authorize]
        public async Task<IActionResult> AddComment([FromForm] AddProjectCommentDto dto)
        {
            return Ok(await _service.AddCommentAsync(dto));
        }
        [HttpDelete("[Action]")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(string projectId, string projectCommentId)
        {
            return Ok(await _service.DeleteCommentAsync(projectId, projectCommentId));
        }
        [HttpPost("[Action]")]
        [Authorize]
        public async Task<IActionResult> AddReply([FromForm] AddProjectReplyDto dto)
        {
            return Ok(await _service.AddReplyAsync(dto));
        }
        [HttpDelete("[Action]")]
        [Authorize]
        public async Task<IActionResult> DeleteReply(string projectId, string projectCommentId, string projectReplyId)
        {
            return Ok(await _service.DeleteReplyAsync(projectId, projectCommentId, projectReplyId));
        }
    }
}
