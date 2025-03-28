using FluentValidation;
using FluentValidation.Results;
using Survey.Infrastructure.DTOs.Poll.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.FluentValidation.PollValidator
{
    public class PollRequestValidator : AbstractValidator<PollRequest>
    {
        public PollRequestValidator()
        {
            RuleFor(P => P.Title)
                .NotEmpty()
                .Length(min: 3, max: 50);

            RuleFor(P => P.Summary)
                .NotEmpty()
                .Length(3, 1500);

            RuleFor(P => P.StartsAt)
                .NotEmpty()
                .GreaterThan(DateOnly.FromDateTime(DateTime.Today));

            RuleFor(P => P)
                .Must(ValidRangeDate)
                .OverridePropertyName("Valid Date")
                .WithMessage("EndsAt date should be greater than StartsAt date");

            RuleFor(P => P.EndsAt)
                .NotEmpty();
        }

        public bool ValidRangeDate(PollRequest request)
        {
            return request.StartsAt <= request.EndsAt;
        }
    }
}
