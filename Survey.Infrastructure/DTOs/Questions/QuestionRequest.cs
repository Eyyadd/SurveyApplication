using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.DTOs.Questions
{
    public class QuestionRequest
    {
        public string Content { get; set; } = string.Empty;
        public List<string> Answers { get; set; } = [];
    }
}
