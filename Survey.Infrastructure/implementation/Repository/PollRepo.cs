using Microsoft.EntityFrameworkCore;
using Survey.Domain.Entities;
using Survey.Domain.Interfaces.IRepository;
using Survey.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.implementation.Repository
{
    public class PollRepo(SurveyDbContext dbContext) : GenericRepository<Poll>(dbContext), IPollRepo
    {
        private readonly SurveyDbContext _DbContext = dbContext;

        public async Task<IEnumerable<Poll?>> GetCurrentPolls(CancellationToken cancellationToken)
        {
            var availablePolls = await _DbContext.Polls
                .AsNoTracking()
                .Where(p => p.IsPublished && (DateOnly.FromDateTime(DateTime.UtcNow) >= p.StartsAt && DateOnly.FromDateTime(DateTime.UtcNow) <= p.EndsAt))
                .ToListAsync(cancellationToken);
            return availablePolls;
        }
        public async Task<bool> IsTitleUnique(string title)
        {
            return !(await _DbContext.Polls.AnyAsync(p => p.Title == title));
        }

        public async Task<bool> isQuestionVotedByUser(int pollId,string userId, CancellationToken cancellationToken)
        {
            return await _DbContext.Votes.AnyAsync(v => v.PollId == pollId && v.UserId == userId);
        }
    }
}
