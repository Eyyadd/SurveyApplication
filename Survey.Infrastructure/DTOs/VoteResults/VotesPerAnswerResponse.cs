using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.DTOs.VoteResults
{
    public class VotesPerAnswerResponse
    {
        public string Answer { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
