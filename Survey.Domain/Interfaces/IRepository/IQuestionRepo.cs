using Survey.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Domain.Interfaces.IRepository
{
    public interface IQuestionRepo : IGenericRepository<Question>
    {
       
        Task<bool> QuestionIsExist(string Content, int PollId, CancellationToken cancellationToken);
        Task<IEnumerable<Question>> GetAllByPollId(int pollId, CancellationToken cancellationToken);
        Task<Question?> GetQuestionByPollId(int pollId, int id, CancellationToken cancellationToken);
        Task<bool> ToggleStatus(int pollId, int id, CancellationToken cancellationToken);
        Task<bool> UpdateByPollId(int pollId, int id, Question entity, CancellationToken cancellationToken);
        Task<IEnumerable<Question>> GetNotVotedQuestion(int pollId, CancellationToken cancellationToken);
        Task<IEnumerable<int>> GetAvailablePollsIDS(int pollId, CancellationToken cancellationToken);
        
    }
}
