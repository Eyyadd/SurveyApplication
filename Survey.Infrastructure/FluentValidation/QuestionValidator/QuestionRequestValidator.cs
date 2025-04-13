using FluentValidation;
using Survey.Domain.Entities;
using Survey.Infrastructure.DTOs.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.FluentValidation.QuestionValidator
{
    public class QuestionRequestValidator : AbstractValidator<QuestionRequest>
    {
        public QuestionRequestValidator()
        {
            RuleFor(q => q.Content)
                .NotEmpty()
                .Length(3, 1000)
                .WithMessage("Question content must be between 3 and 1000 characters long.");


            RuleFor(q => q.Answers)
                .NotNull();

            RuleFor(q => q.Answers)
                .Must(answers => answers.Count > 1)
                .When(a => a.Answers is not null)
                .WithMessage("At least 2 answers are required.");

            RuleFor(q => q.Answers)
            .Must(answers => answers.Distinct().Count() == answers.Count)
            .When(a => a.Answers is not null)
            .WithMessage("Answers must be unique.");

        }
        //private bool AnswersUniqueness(List<string> answers)
        //{
        //    return 
        //}
    }
}
