using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.DTOs.VoteResults
{
    public class VotesPerQuestionResponse
    {
        public string Question { get; set; } = string.Empty;
        public IEnumerable<VotesPerAnswerResponse> SelectedAnswers { get; set; } = default!;
    }
}
