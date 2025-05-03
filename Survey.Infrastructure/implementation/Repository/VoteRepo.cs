using Survey.Domain.Entities;
using Survey.Domain.Interfaces.IRepository;
using Survey.Infrastructure.Core;

namespace Survey.Infrastructure.implementation.Repository
{
    public class VoteRepo(SurveyDbContext surveyDbContext) : GenericRepository<Vote>(surveyDbContext), IVoteRepo
    {
    }
}
