using FluentValidation;
using Survey.Infrastructure.DTOs.Auth.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.FluentValidation.AuthValidator
{
    public class RegisterRequestValidator:AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator() {

            RuleFor(U => U.FirstName)
                .NotEmpty()
                .Length(3, 20);

            RuleFor(U => U.LastName)
                .NotEmpty()
                .Length(3, 20);

            RuleFor(U => U.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(U => U.Password)
                .NotEmpty();

            RuleFor(U => U.UserName)
                .NotEmpty();
        }
    }
}
