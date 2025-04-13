using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Survey.Infrastructure.DTOs.Poll.Requests;
using Survey.Infrastructure.Extensions;
using Survey.Infrastructure.IService;
using System.Threading;

namespace Survey.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PollsController(IPollService pollService) : ControllerBase
    {
        private readonly IPollService _PollService = pollService;


        [HttpGet("")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _PollService.GetAllAsync(cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await _PollService.GetByIdAsync(id, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.StandardError(StatusCodes.Status400BadRequest);
        }

        [HttpPost("")]
        public async Task<IActionResult> Add([FromBody] PollRequest entity, CancellationToken cancellationToken)
        {
            var result = await _PollService.AddAsync(entity, cancellationToken);

            return result.IsSuccess ? Ok() : result.StandardError(StatusCodes.Status400BadRequest);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PollRequest entity, CancellationToken cancellationToken)
        {
            var result = await _PollService.UpdateAsync(id, entity, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.StandardError(StatusCodes.Status400BadRequest);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await _PollService.DeleteAsync(id, cancellationToken);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPut("{id}/Toggle-IsPublished")]
        public async Task<IActionResult> ToggleIsPublished(int id, CancellationToken cancellationToken)
        {
            var result = await _PollService.ToggleIsPublished(id, cancellationToken);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
    }
}
