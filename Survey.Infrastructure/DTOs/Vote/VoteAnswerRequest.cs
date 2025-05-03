using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.DTOs.Vote
{
    public class VoteAnswerRequest
    {
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
    }
}
