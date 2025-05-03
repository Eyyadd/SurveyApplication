using Survey.Infrastructure.Abstractions;
using Survey.Infrastructure.DTOs.Poll.Requests;
using Survey.Infrastructure.DTOs.Poll.Responses;
using Survey.Infrastructure.DTOs.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.IService
{
    public interface IQuestionService
    {
        Task<Result<IEnumerable<QuestionResponse>>> GetAllAsync(int pollId,CancellationToken cancellationToken);
        Task<Result<IEnumerable<QuestionResponse>>> GetAvailableAsync(int pollId,string userId,CancellationToken cancellationToken);
        Task<Result<QuestionResponse>> GetByIdAsync(int pollId, int id, CancellationToken cancellationToken);
        Task<Result<QuestionResponse>> AddAsync(int pollId,QuestionRequest entity, CancellationToken cancellationToken);
        Task<Result> UpdateAsync(int pollId,int id, QuestionRequest entity, CancellationToken cancellationToken);
        Task<Result> DeleteAsync(int pollId,int id, CancellationToken cancellationToken);
        Task<Result> ToggleStatus(int pollId,int id, CancellationToken cancellationToken);
        
    }
}
