using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.Abstractions
{
    public class Error(string Code, string Message)
    {
        public string Code { get; } = Code;
        public string Message { get; } = Message;
    }
}
