using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Survey.Infrastructure.DTOs.Poll.Requests;
using Survey.Infrastructure.DTOs.Questions;
using Survey.Infrastructure.Extensions;
using Survey.Infrastructure.implementation.Service;
using Survey.Infrastructure.IService;

namespace Survey.API.Controllers
{
    [Route("api/Polls/{PollId}/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionsController(IQuestionService questionService) : ControllerBase
    {
        private readonly IQuestionService _QuestionService = questionService;

        [HttpGet("")]
        public async Task<IActionResult> GetAll(int pollId,CancellationToken cancellationToken)
        {
            var result = await _QuestionService.GetAllAsync(pollId,cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int pollId,[FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _QuestionService.GetByIdAsync(pollId,id, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.StandardError(StatusCodes.Status400BadRequest);
        }

        [HttpPost("")]
        public async Task<IActionResult> Add([FromRoute] int pollId,[FromBody] QuestionRequest entity, CancellationToken cancellationToken)
        {
            var result = await _QuestionService.AddAsync(pollId,entity, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.StandardError(StatusCodes.Status400BadRequest);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int pollId,[FromRoute] int id, [FromBody] QuestionRequest entity, CancellationToken cancellationToken)
        {
            var result = await _QuestionService.UpdateAsync(pollId,id, entity, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.StandardError(StatusCodes.Status400BadRequest);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int pollId, [FromRoute]int id, CancellationToken cancellationToken)
        {
            var result = await _QuestionService.DeleteAsync(pollId,id, cancellationToken);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPut("{id}/Toggle-Status")]
        public async Task<IActionResult> ToggleStatus([FromRoute] int pollId,[FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _QuestionService.ToggleStatus(pollId, id, cancellationToken);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
    }
}
