using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Domain.Entities
{
    public class VoteAnswer :BaseEntity
    {
        public int VoteId { get; set; }
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }

        public Vote Vote { get; set; } = default!;
        public Question Question { get; set; } = default!;
        public Answer Answer { get; set; } = default!;
    }
}
