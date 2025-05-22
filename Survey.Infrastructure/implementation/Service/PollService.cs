using Hangfire;
using Mapster;
using MapsterMapper;
using Survey.Domain.Entities;
using Survey.Domain.Interfaces.IRepository;
using Survey.Infrastructure.Abstractions;
using Survey.Infrastructure.DTOs.Poll.Requests;
using Survey.Infrastructure.DTOs.Poll.Responses;
using Survey.Infrastructure.Errors;
using Survey.Infrastructure.IService;

namespace Survey.Infrastructure.implementation.Service
{
    public class PollService(IPollRepo PollRepo , IMapper mapper,INotificationService notificationService) : IPollService
    {
        private readonly IPollRepo _pollRepo = PollRepo;
        private readonly IMapper _Mapper = mapper;
        private readonly INotificationService _NotificationService = notificationService;

        public async Task<Result> AddAsync(PollRequest entity, CancellationToken cancellationToken)
        {
            var Poll = entity.Adapt<Poll>();
            var result = await _pollRepo.Add(Poll, cancellationToken);

            if (result == 0)
                return Result.Failure(PollErrors.PollCreationFailed);

            return Result.Success();
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var PollIsExist = await _pollRepo.IsExist(id, cancellationToken);
            if (PollIsExist)
            {
                var oldPoll = await _pollRepo.GetById(id, cancellationToken);
                var result = await _pollRepo.Delete(oldPoll!, cancellationToken);
                if (result == 0)
                    return Result.Failure(PollErrors.PollDeletionFailed);

                return Result.Success();
            }
            return Result.Failure(PollErrors.PollNotFound);

        }

        public async Task<Result<IEnumerable<PollResponse>>> GetAllAsync(CancellationToken cancellationToken)
        {
            var Polls = await _pollRepo.GetAll(cancellationToken);

            if (Polls is null)
                return Result.Failure<IEnumerable<PollResponse>>(PollErrors.PollNotFound);

            var PollsResponse = Polls.Adapt<IEnumerable<PollResponse>>();
            return Result.Success(PollsResponse);

        }

        public async Task<Result<PollResponse>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var poll = await _pollRepo.GetById(id, cancellationToken);

            if (poll is null)
                return Result.Failure<PollResponse>(PollErrors.PollNotFound);

            var pollResponse = poll.Adapt<PollResponse>();
            return Result.Success(pollResponse);
        }

        public async Task<Result<PollResponse>> UpdateAsync(int id, PollRequest entity, CancellationToken cancellationToken)
        {
            var pollIsExist = await _pollRepo.GetByIdWithoutTracking(id, cancellationToken);

            if (pollIsExist is not null)
            {
                var poll = entity.Adapt<Poll>();
                poll.CreatedByUserId = pollIsExist.CreatedByUserId;
                var IsUpdated = await _pollRepo.Update(id, poll, cancellationToken);
                if(IsUpdated == 0)
                    return Result.Failure<PollResponse>(PollErrors.PollUpdateFailed);

                var updatedPoll = poll.Adapt<PollResponse>();
                return Result.Success(updatedPoll);
            }
            return Result.Failure<PollResponse>(PollErrors.PollNotFound);

        }

        public async Task<Result> ToggleIsPublished(int id, CancellationToken cancellationToken)
        {
            var PollIsExist = await _pollRepo.IsExist(id, cancellationToken);
            if (PollIsExist)
            {
                var Poll = await _pollRepo.GetById(id, cancellationToken);
                Poll!.IsPublished = !Poll.IsPublished;
                var result = await _pollRepo.SaveChanges(cancellationToken);

                if(Poll.IsPublished && Poll.StartsAt == DateOnly.FromDateTime(DateTime.UtcNow))
                {
                    BackgroundJob.Enqueue(() => _NotificationService.SendNewPollNotification(Poll.Id));
                }
                return Result.Success();
            }
            return Result.Failure(PollErrors.PollNotFound);
        }

        public async Task<Result<IEnumerable<PollResponse>>> GetAvailablePolls(CancellationToken cancellationToken)
        {
            // it should be isPublished and the current date between start and end date
            var currentPolls = await _pollRepo.GetCurrentPolls(cancellationToken);
            var PollRespons = currentPolls.Adapt<IEnumerable<PollResponse>>();

            return Result.Success(PollRespons);
        }
    }
}
