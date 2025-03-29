using FluentValidation;
using Survey.Domain.Entities;
using Survey.Infrastructure.DTOs.Auth.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.FluentValidation.AuthValidator
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(U => U.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(U => U.Password)
                .NotEmpty();
        }
    }
}
