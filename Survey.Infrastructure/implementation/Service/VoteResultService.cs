using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Survey.Infrastructure.Abstractions;
using Survey.Infrastructure.Core;
using Survey.Infrastructure.DTOs.VoteResults;
using Survey.Infrastructure.Errors;
using Survey.Infrastructure.IService;

namespace Survey.Infrastructure.implementation.Service
{
    public class VoteResultService(SurveyDbContext dbContext) : IVoteResultService
    {
        private readonly SurveyDbContext _DbContext = dbContext;

        public async Task<Result<PollVoteResponse>> GetPollVotesAsync(int pollId, CancellationToken cancellationToken)
        {
            var VotesResult = await _DbContext.Polls
                .Where(p => p.Id == pollId)
                .Select(p => new PollVoteResponse
                {
                    Title = p.Title,
                    Votes = p.Votes.Select(v => new VoteResponse
                    {
                        VoteDate = v.SubmittedOn,
                        VoterName = v.User.FirstName + ' ' + v.User.LastName,
                        Answers = v.voteAnswers.Select(vs => new QuestionAnswerResponse
                        {
                            Answer = vs.Answer.Content,
                            Question = vs.Question.Content
                        })
                    })
                }).SingleOrDefaultAsync(cancellationToken);

            return VotesResult is null ?
                 Result.Failure<PollVoteResponse>(PollErrors.PollNotFound) :
                 Result.Success(VotesResult);

        }

        public async Task<Result<IEnumerable<VotesPerDayResponse>>> GetPollVotesPerDayAsync(int pollId, CancellationToken cancellationToken)
        {

            var VotesPerDay = await _DbContext.Votes
                .Where(v => v.PollId == pollId)
                .GroupBy(x => new { Date = DateOnly.FromDateTime(x.SubmittedOn) })
                .Select(g => new VotesPerDayResponse
                {
                    Date = g.Key.Date,
                    NumberOfVotes = g.Count()
                }).ToListAsync(cancellationToken);

            return VotesPerDay is null ?
                 Result.Failure<IEnumerable<VotesPerDayResponse>>(PollErrors.PollNotFound) :
                 Result.Success<IEnumerable<VotesPerDayResponse>>(VotesPerDay);

        }


        public async Task<Result<IEnumerable<VotesPerQuestionResponse>>> GetVotesPerAnswerAsync(int pollId, CancellationToken cancellationToken)
        {

            var VotesPerAnswer = await _DbContext.VoteAnswers
                .Where(v => v.Vote.PollId == pollId)
                .Select(v => new VotesPerQuestionResponse
                {
                    Question = v.Question.Content,
                    SelectedAnswers = v.Question.voteAnswers
                                       .GroupBy(g => new { Answers = g.Answer.Content })
                                       .Select(g => new VotesPerAnswerResponse
                                       {
                                           Answer = g.Key.Answers,
                                           Count = g.Count()
                                       }).ToList()

                }).ToListAsync(cancellationToken);
                

            return VotesPerAnswer is null ?
                 Result.Failure<IEnumerable<VotesPerQuestionResponse>>(PollErrors.PollNotFound) :
                 Result.Success<IEnumerable<VotesPerQuestionResponse>>(VotesPerAnswer);

        }
    }
}

//Question - array of answers [{content - number of selected}]


