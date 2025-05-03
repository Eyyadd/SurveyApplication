using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.DTOs.VoteResults
{
    public class PollVoteResponse
    {
        public string Title { get; set; } = string.Empty;
        public IEnumerable<VoteResponse> Votes { get; set; } = default!;
    }
}
