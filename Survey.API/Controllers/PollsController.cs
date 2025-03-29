using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Survey.Infrastructure.DTOs.Poll.Requests;
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
            var Polls = await _PollService.GetAllAsync(cancellationToken);
            if (Polls is null)
                return NotFound();
            return Ok(Polls);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id,CancellationToken cancellationToken)
        {
            var Poll = await _PollService.GetByIdAsync(id, cancellationToken);
            if (Poll is null)
                return NotFound();
            return Ok(Poll);
        }

        [HttpPost("")]
        public async Task<IActionResult> Add([FromBody] PollRequest entity,CancellationToken cancellationToken)
        {
            var result = await _PollService.AddAsync(entity, cancellationToken);
            
            return result != 0 ? CreatedAtAction(nameof(GetById), new { id = result }, entity) : BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PollRequest entity,CancellationToken cancellationToken)
        {
            var result = await _PollService.UpdateAsync(id, entity, cancellationToken);
            return result is not null ? Ok(entity) : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id,CancellationToken cancellationToken)
        {
            var result = await _PollService.DeleteAsync(id, cancellationToken);

            return result == 1 ? Ok() : BadRequest();
        }

        [HttpPut("{id}/Toggle-IsPublished")]
        public async Task<IActionResult> ToggleIsPublished(int id,CancellationToken cancellationToken)
        {
            var result = await _PollService.ToggleIsPublished(id, cancellationToken);
            return result ? NoContent() : NotFound();
        }
    }
}
