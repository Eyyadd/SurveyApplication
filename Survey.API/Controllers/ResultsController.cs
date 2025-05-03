using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Survey.Infrastructure.Extensions;
using Survey.Infrastructure.IService;

namespace Survey.API.Controllers
{
    [Route("api/Poll/{pollId}/[controller]")]
    [ApiController]
    //[Authorize]
    public class ResultsController(IVoteResultService voteResult) : ControllerBase
    {
        private readonly IVoteResultService _VoteResult = voteResult;

        [HttpGet("")]
        public async Task<IActionResult> PollVotes([FromRoute] int pollId, CancellationToken cancellationToken)
        {
            var result = await _VoteResult.GetPollVotesAsync(pollId, cancellationToken);
            if (result.IsSuccess)
                return Ok(result.Value);
            return result.StandardError(StatusCodes.Status404NotFound);
        }

        [HttpGet("PerDay")]
        public async Task<IActionResult> PollVotesPerDay([FromRoute] int pollId, CancellationToken cancellationToken)
        {
            var result = await _VoteResult.GetPollVotesPerDayAsync(pollId, cancellationToken);
            if (result.IsSuccess)
                return Ok(result.Value);
            return result.StandardError(StatusCodes.Status404NotFound);
        }

        [HttpGet("PerAnswer")]
        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> PollVotesPerAnswer([FromRoute] int pollId, CancellationToken cancellationToken)
        {
            var result = await _VoteResult.GetVotesPerAnswerAsync(pollId, cancellationToken);
            if (result.IsSuccess)
                return Ok(result.Value);
            return result.StandardError(StatusCodes.Status404NotFound);
        }
    }
}
