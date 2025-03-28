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

        public async Task<int> AddAsync(PollRequest entity)
        {
            var Poll = entity.Adapt<Poll>();
            var result = await _pollRepo.Add(Poll);
            return result;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var PollIsExist = await _pollRepo.IsExist(id);
            if (PollIsExist)
            {
                var oldPoll = await _pollRepo.GetById(id);
                return await _pollRepo.Delete(oldPoll);
            }
            return 0;

        }

        public async Task<IEnumerable<PollResponse?>> GetAllAsync()
        {
            var Polls = await _pollRepo.GetAll();

            if (Polls is null)
                return null!;

            var PollsResponse = Polls.Adapt<IEnumerable<PollResponse>>();
            return PollsResponse;

        }

        public async Task<PollResponse?> GetByIdAsync(int id)
        {
            var poll = await _pollRepo.GetById(id);

            if (poll is null)
                return null!;

            var pollResponse = poll.Adapt<PollResponse>();
            return pollResponse;
        }

        public async Task<PollResponse> UpdateAsync(int id, PollRequest entity)
        {
            var pollIsExist = await _pollRepo.IsExist(id);

            if (pollIsExist)
            {
                var poll = entity.Adapt<Poll>();
                var IsUpdated = await _pollRepo.Update(id,poll);
                return IsUpdated  != 0 ? entity.Adapt<PollResponse>() : null!;
            }
            return null!;

        }
    }
}
