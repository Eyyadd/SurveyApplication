using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Survey.Infrastructure.Abstractions;
using Survey.Infrastructure.DTOs.Vote;
using Survey.Infrastructure.Errors;
using Survey.Infrastructure.Extensions;
using Survey.Infrastructure.IService;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Survey.API.Controllers
{
    [Route("api/Poll/{PollId}/[controller]")]
    [ApiController]
    [Authorize]
    public class VotesController(IQuestionService questionService,IVoteService voteService) : ControllerBase
    {
        private readonly IQuestionService _QuestionService = questionService;
        private readonly IVoteService _VoteService = voteService;

        [HttpGet("")]
        public async Task<IActionResult> Start([FromRoute] int pollId, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var result = await _QuestionService.GetAvailableAsync(pollId, userId!, cancellationToken);
            if(result.IsSuccess)
                return Ok(result.Value);

            return result.Error.Equals(PollErrors.PollNotFound) ?

                result.StandardError(StatusCodes.Status404NotFound) :

                result.StandardError(StatusCodes.Status409Conflict);
        }

        [HttpPost("")]
        public async Task<IActionResult> AddAsync([FromRoute] int pollId, VoteRequest request ,CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var result = await _VoteService.AddAsync(pollId, userId!, request, cancellationToken);
            if (result.IsSuccess)
                return Created();
            return result.StandardError(StatusCodes.Status400BadRequest);
        }
    }
}
