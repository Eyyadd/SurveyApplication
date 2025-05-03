using Survey.Domain.Entities;
using Survey.Domain.Interfaces.IRepository;
using Survey.Infrastructure.Abstractions;
using Survey.Infrastructure.DTOs.Questions;
using Survey.Infrastructure.DTOs.Vote;
using Survey.Infrastructure.Errors;
using Survey.Infrastructure.implementation.Repository;
using Survey.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.implementation.Service
{
    public class VoteService (IVoteRepo voteRepo,IPollRepo pollRepo,IQuestionRepo questionRepo ): IVoteService
    {
        private readonly IVoteRepo _VoteRepo = voteRepo;
        private readonly IPollRepo _PollRepo = pollRepo;
        private readonly IQuestionRepo _QuestionRepo = questionRepo;

        public async Task<Result> AddAsync(int pollId, string userId, VoteRequest request, CancellationToken cancellationToken)
        {
            var isPollExist = await _PollRepo.IsExist(pollId, cancellationToken);
            if (!isPollExist)
                return Result.Failure(PollErrors.PollNotFound);

            var hasVoted = await _PollRepo.isQuestionVotedByUser(pollId, userId, cancellationToken);
            if (hasVoted)
                return Result.Failure(VoteErrors.UserAlreadyVote);

            var AvailableQuestion = await _QuestionRepo.GetAvailablePollsIDS(pollId, cancellationToken);
            var availableAnswers = request.Answers.Select(x => x.QuestionId);
            if (!availableAnswers.SequenceEqual(AvailableQuestion))
            {
                return Result.Failure(VoteErrors.InvalidOperation);
            }

            var vote = new Vote
            {
                PollId = pollId,
                UserId = userId,
                voteAnswers = request.Answers.Select(a => new VoteAnswer { QuestionId = a.QuestionId, AnswerId = a.AnswerId }).ToList()
            };

            await _VoteRepo.Add(vote, cancellationToken);
            return Result.Success();

        }
    }
}
