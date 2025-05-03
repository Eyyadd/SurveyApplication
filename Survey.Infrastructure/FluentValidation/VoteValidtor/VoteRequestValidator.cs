using FluentValidation;
using Survey.Infrastructure.DTOs.Vote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.FluentValidation.VoteValidtor
{
    public class VoteRequestValidator : AbstractValidator<VoteRequest>
    {
        public VoteRequestValidator()
        {
            RuleFor(a => a.Answers)
                .NotEmpty();
        }
    }
}
