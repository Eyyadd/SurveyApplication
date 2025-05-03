using Survey.Infrastructure.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.Errors
{
    public static class VoteErrors
    {
        public static Error UserAlreadyVote = new Error("Question has been Voted Before", "this user vote on this question before");
        public static Error InvalidOperation = new Error("Invalid Vote", "this vote is invalid");
    }
}
