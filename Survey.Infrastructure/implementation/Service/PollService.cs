using Survey.Domain.Entities;
using Survey.Domain.Interfaces.IRepository;
using Survey.Infrastructure.IService;
using Survey.Infrastructure.DTOs.Poll.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using Survey.Infrastructure.DTOs.Poll.Responses;

namespace Survey.Infrastructure.implementation.Service
{
    public class PollService(IGenericRepository<Poll> PollRepo) : IPollService
    {
        private readonly IGenericRepository<Poll> _pollRepo = PollRepo;

        public async Task<int> AddAsync(PollRequest entity, CancellationToken cancellationToken)
        {
            var Poll = entity.Adapt<Poll>();
            var result = await _pollRepo.Add(Poll, cancellationToken);
            return result;
        }

        public async Task<int> DeleteAsync(int id , CancellationToken cancellationToken)
        {
            var PollIsExist = await _pollRepo.IsExist(id, cancellationToken);
            if (PollIsExist)
            {
                var oldPoll = await _pollRepo.GetById(id, cancellationToken);
                return await _pollRepo.Delete(oldPoll, cancellationToken);
            }
            return 0;

        }

        public async Task<IEnumerable<PollResponse?>> GetAllAsync(CancellationToken cancellationToken)
        {
            var Polls = await _pollRepo.GetAll(cancellationToken);

            if (Polls is null)
                return null!;

            var PollsResponse = Polls.Adapt<IEnumerable<PollResponse>>();
            return PollsResponse;

        }

        public async Task<PollResponse?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var poll = await _pollRepo.GetById(id, cancellationToken);

            if (poll is null)
                return null!;

            var pollResponse = poll.Adapt<PollResponse>();
            return pollResponse;
        }

        public async Task<PollResponse> UpdateAsync(int id, PollRequest entity, CancellationToken cancellationToken)
        {
            var pollIsExist = await _pollRepo.IsExist(id, cancellationToken);

            if (pollIsExist)
            {
                var poll = entity.Adapt<Poll>();
                var IsUpdated = await _pollRepo.Update(id,poll, cancellationToken);
                return IsUpdated  != 0 ? entity.Adapt<PollResponse>() : null!;
            }
            return null!;

        }
    }
}
