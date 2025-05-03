using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.DTOs.Vote
{
    public class VoteRequest
    {
        public IEnumerable<VoteAnswerRequest> Answers { get; set; } = default!;
    }
}
