using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.DTOs.Poll.Responses
{
    public class PollResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public bool IsPublished { get; set; }
        public DateOnly StartsAt { get; set; }

        public DateOnly EndsAt { get; set; }
    }
}
