using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Survey.Infrastructure.DTOs.VoteResults
{
    public class VotesPerDayResponse
    {
        public  DateOnly Date { get; set; }

        [JsonPropertyName("No of Votes")]
        public int NumberOfVotes { get; set; }
    }
}
