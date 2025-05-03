using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.DTOs.VoteResults
{
    public class VoteResponse
    {
        public string VoterName { get; set; } = string.Empty;
        public DateTime VoteDate { get; set; }
        public IEnumerable<QuestionAnswerResponse> Answers { get; set; } = default!;
    }
}
