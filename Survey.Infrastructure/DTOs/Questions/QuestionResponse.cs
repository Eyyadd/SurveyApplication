using Survey.Infrastructure.DTOs.Answers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.DTOs.Questions
{
    public class QuestionResponse
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;

        public IEnumerable<AnswerResponse> Answers { get; set; } = [];

    }
}
