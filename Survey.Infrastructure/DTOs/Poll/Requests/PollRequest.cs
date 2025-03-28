using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.DTOs.Poll.Requests
{
    public class PollRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public DateOnly StartsAt { get; set; }
        public DateOnly EndsAt { get; set; }
        public bool IsPublished { get; set; }
    }
}
