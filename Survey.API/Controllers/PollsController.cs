using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Survey.Infrastructure.DTOs.Poll.Requests;
using Survey.Infrastructure.IService;

namespace Survey.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PollsController(IPollService pollService) : ControllerBase
    {
        private readonly IPollService _PollService = pollService;


        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var Polls = await _PollService.GetAllAsync();
            if (Polls is null)
                return NotFound();
            return Ok(Polls);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var Poll = await _PollService.GetByIdAsync(id);
            if (Poll is null)
                return NotFound();
            return Ok(Poll);
        }

        [HttpPost("")]
        public async Task<IActionResult> Add([FromBody] PollRequest entity)
        {
            var result = await _PollService.AddAsync(entity);
            
            return result == 1 ? CreatedAtAction(nameof(GetById), new { id = result }, entity) : BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PollRequest entity)
        {
            var result = await _PollService.UpdateAsync(id, entity);
            return result is not null ? Ok(entity) : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _PollService.DeleteAsync(id);

            return result == 1 ? Ok() : BadRequest();
        }
    }
}
