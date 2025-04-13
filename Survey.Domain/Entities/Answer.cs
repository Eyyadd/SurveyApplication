using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Domain.Entities
{
    public class Answer : BaseEntity
    {
        public string Content { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public int QuestionId { get; set; }
        public Question Question { get; set; } = new();
    }
}
