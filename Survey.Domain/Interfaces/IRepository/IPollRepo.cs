using Survey.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Domain.Interfaces.IRepository
{
    public interface IPollRepo : IGenericRepository<Poll>
    {
        Task<IEnumerable<Poll?>> GetCurrentPolls(CancellationToken cancellationToken);
        Task<bool> IsTitleUnique(string title);
        Task<bool> isQuestionVotedByUser(int pollId, string userId, CancellationToken cancellationToken);

    }
}
