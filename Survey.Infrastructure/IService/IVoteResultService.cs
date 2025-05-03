using Survey.Infrastructure.Abstractions;
using Survey.Infrastructure.DTOs.VoteResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.IService
{
    public interface IVoteResultService
    {
        Task<Result<PollVoteResponse>> GetPollVotesAsync(int pollId, CancellationToken cancellationToken);
        Task<Result<IEnumerable<VotesPerDayResponse>>> GetPollVotesPerDayAsync(int pollId, CancellationToken cancellationToken);
        Task<Result<IEnumerable<VotesPerQuestionResponse>>> GetVotesPerAnswerAsync(int pollId, CancellationToken cancellationToken);
    }
}
