using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Domain.Entities
{
    public class Question : BaseEntity
    {
        public string Content { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public int PollId { get; set; }
        public Poll Poll { get; set; } = default!;
        public ICollection<Answer> Answers { get; set; } = [];
    }
}
