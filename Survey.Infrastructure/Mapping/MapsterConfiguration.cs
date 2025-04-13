using Mapster;
using Survey.Domain.Entities;
using Survey.Infrastructure.DTOs.Answers;
using Survey.Infrastructure.DTOs.Poll.Responses;
using Survey.Infrastructure.DTOs.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.Mapping
{
    public class MapsterConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<PollResponse, Poll>()
                .Ignore(x => x.CreatedAt)
                .Ignore(x => x.CreatedBy)
                .Ignore(x => x.CreatedByUserId);


            config.NewConfig<QuestionRequest, Question>()
                .Map(dest => dest.Answers, src => src.Answers.Select(ans => new Answer { Content = ans }).ToList());

            config.NewConfig<AnswerResponse, Answer>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Content, src => src.Content);
        }
    }
}
