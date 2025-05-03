using FluentValidation;
using Survey.Infrastructure.DTOs.Auth.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.FluentValidation.AuthValidator
{
    public class ConfirmEmailRequestValidator : AbstractValidator<ConfirmEmailRequest>
    {
        public ConfirmEmailRequestValidator()
        {
            RuleFor(e => e.UserId)
                .NotEmpty();

            RuleFor(e => e.Code)
                .NotEmpty();

        }
    }
}
