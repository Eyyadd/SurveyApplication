using Mapster;
using Survey.Domain.Entities;
using Survey.Domain.Interfaces.IRepository;
using Survey.Infrastructure.Abstractions;
using Survey.Infrastructure.DTOs.Questions;
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
    public class QuestionService(IQuestionRepo questionRepo, IPollRepo pollRepo, IGenericRepository<Answer> answerRepo) : IQuestionService
    {
        private readonly IQuestionRepo _QuestionRepo = questionRepo;
        private readonly IPollRepo _PollRepo = pollRepo;
        private readonly IGenericRepository<Answer> _AnswerRepo = answerRepo;

        public async Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest entity, CancellationToken cancellationToken)
        {
            //check if there's a poll with that poll Id 
            var pollIsExist = await _PollRepo.IsExist(pollId, cancellationToken);
            if (!pollIsExist)
                return Result.Failure<QuestionResponse>(PollErrors.PollNotFound);

            //check if the question is already exist
            var questionIsExist = await _QuestionRepo.QuestionIsExist(entity.Content, pollId, cancellationToken);
            if (questionIsExist)
                return Result.Failure<QuestionResponse>(QuestionErrors.QuestionAlreadyExists);



            //create the question
            var question = entity.Adapt<Question>();
            question.PollId = pollId;

            var result = await _QuestionRepo.Add(question, cancellationToken);
            if (result == 0)
                return Result.Failure<QuestionResponse>(QuestionErrors.QuestionCreationFailed);

            var questionResponse = question.Adapt<QuestionResponse>();
            return Result.Success(questionResponse);

        }

        public Task<Result> DeleteAsync(int pollId, int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<IEnumerable<QuestionResponse>>> GetAllAsync(int pollId, CancellationToken cancellationToken)
        {
            var questions = await _QuestionRepo.GetAllByPollId(pollId, cancellationToken);

            var questionResponses = questions.Adapt<IEnumerable<QuestionResponse>>();
            return Result.Success(questionResponses);
        }

        public async Task<Result<IEnumerable<QuestionResponse>>> GetAvailableAsync(int pollId, string userId, CancellationToken cancellationToken)
        {
            //first check is pollId Exist 
            var isPollExist = await _PollRepo.IsExist(pollId, cancellationToken);
            if (!isPollExist)
                return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.PollNotFound);
            //then check that the current user has already voted on this poll ? 
            var hasVoted = await _PollRepo.isQuestionVotedByUser(pollId, userId, cancellationToken);
            if(hasVoted)
                return Result.Failure<IEnumerable<QuestionResponse>>(VoteErrors.UserAlreadyVote);

            var question = await _QuestionRepo.GetNotVotedQuestion(pollId, cancellationToken);
           
            var result = question.Adapt<IEnumerable<QuestionResponse>>();

            return Result.Success(result);


        }

        public async Task<Result<QuestionResponse>> GetByIdAsync(int pollId, int id, CancellationToken cancellationToken)
        {
            var question = await _QuestionRepo.GetQuestionByPollId(pollId, id, cancellationToken);
            if (question == null)
                return Result.Failure<QuestionResponse>(QuestionErrors.QuestionNotFound);
            var questionResponse = question.Adapt<QuestionResponse>();
            return Result.Success(questionResponse);
        }

        public async Task<Result> ToggleStatus(int pollId, int id, CancellationToken cancellationToken)
        {
            var questionIsExist = await _QuestionRepo.GetQuestionByPollId(pollId, id, cancellationToken);
            if (questionIsExist == null)
                return Result.Failure(QuestionErrors.QuestionNotFound);

            var isStatusChanged = await _QuestionRepo.ToggleStatus(pollId, id, cancellationToken);
            if (!isStatusChanged)
                return Result.Failure(QuestionErrors.QuestionStatusChangeFailed);

            return Result.Success();
        }

        public async Task<Result> UpdateAsync(int pollId, int id, QuestionRequest entity, CancellationToken cancellationToken)
        {
            //check if there's a poll with that poll Id
            var pollIsExist =  await _PollRepo.IsExist(pollId, cancellationToken);
            if(!pollIsExist)
                return Result.Failure<QuestionResponse>(PollErrors.PollNotFound);
            //check if the question is already exist
            var questionIsExist = await _QuestionRepo.GetQuestionByPollId(pollId, id, cancellationToken);
            if (questionIsExist == null)
                return Result.Failure<QuestionResponse>(QuestionErrors.QuestionNotFound);
            //map the entity to the question
            var question = entity.Adapt<Question>();
            //update the question 
            var result = await _QuestionRepo.UpdateByPollId(pollId,id, question, cancellationToken);
            if (!result)
                return Result.Failure<QuestionResponse>(QuestionErrors.QuestionUpdateFailed);

            return Result.Success();
        }



    }
}
