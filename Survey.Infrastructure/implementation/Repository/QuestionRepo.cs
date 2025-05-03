using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Survey.Domain.Entities;
using Survey.Domain.Interfaces.IRepository;
using Survey.Infrastructure.Core;
using Survey.Infrastructure.DTOs.Answers;
using Survey.Infrastructure.DTOs.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.implementation.Repository
{
    public class QuestionRepo(SurveyDbContext dbContext) : GenericRepository<Question>(dbContext), IQuestionRepo
    {
        private readonly SurveyDbContext _DbContext = dbContext;

        public async Task<IEnumerable<Question>> GetAllByPollId(int pollId, CancellationToken cancellationToken)
        {
            var questions = await _DbContext.Questions
                .Where(x => x.PollId == pollId)
                .Include(x => x.Answers)
                .Select(a => new Question
                {
                    Id = a.Id,
                    Content = a.Content,
                    Answers = a.Answers.Select(x => new Answer
                    {
                        Id = x.Id,
                        Content = x.Content,
                    }).ToList(),
                })
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            return questions;
        }

        public async Task<bool> ToggleStatus(int pollId, int id, CancellationToken cancellationToken)
        {
            var question = await _DbContext.Questions
                .Where(x => x.PollId == pollId && x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            question!.IsActive = !question.IsActive;
            await SaveChanges(cancellationToken);

            return true;
        }


        public async Task<Question?> GetQuestionByPollId(int pollId, int id, CancellationToken cancellationToken)
        {
            var question = await _DbContext.Questions
                .Where(x => x.PollId == pollId && x.Id == id)
                .Include(x => x.Answers)
                .Select(a => new Question
                {
                    Id = a.Id,
                    Content = a.Content,
                    Answers = a.Answers.Select(x => new Answer
                    {
                        Id = x.Id,
                        Content = x.Content,
                    }).ToList(),
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
            return question;
        }

        public async Task<bool> QuestionIsExist(string Content, int PollId, CancellationToken cancellationToken)
        {
            var questionIsExist = await _DbContext.Questions
                .AnyAsync(x => x.Content == Content && x.PollId == PollId, cancellationToken);

            return questionIsExist;
        }

        public async Task<bool> UpdateByPollId(int pollId, int id, Question entity, CancellationToken cancellationToken)
        {
            var questionIsExist = await _DbContext.Questions.AnyAsync(q => q.PollId == pollId && q.Id != q.Id && entity.Content == q.Content);

            if(!questionIsExist)
            {
                var question = await _DbContext.Questions
                    .Include(q => q.Answers)
                    .SingleOrDefaultAsync(q => q.Id == id && q.PollId == pollId, cancellationToken);

                if (question is null)
                    return false;

                question.Content = entity.Content;

                // answers section 1- get current answer 2- convert new answer into domain answer 3- deactivate 'delete' the answers which not in the new answers  - update the new answers
                var currentAnswers = question.Answers.Select(a => a.Content).ToList();

                var newAnswers = entity.Answers.ExceptBy(currentAnswers, a => a.Content).ToList();

                newAnswers.ForEach(a =>
                {
                    question.Answers.Add(new Answer { Content = a.Content });
                });

                // deactivate the answers which not in the entity 
                question.Answers.ToList().ForEach(answer =>
                {
                    answer.IsActive = entity.Answers.Any(a => a.Content == answer.Content);
                });

                await _DbContext.SaveChangesAsync(cancellationToken);

                return true;

            }

            return false;

        }

        public async Task<IEnumerable<Question>> GetNotVotedQuestion(int pollId, CancellationToken cancellationToken)
        {
            var result = await _DbContext.Questions
                 .Where(q => q.PollId == pollId && q.IsActive)
                 .Include(a => a.Answers)
                 .Select(a => new Question
                 {
                     Id = a.Id,
                     Content = a.Content,
                     Answers = a.Answers.Where(a => a.IsActive)
                     .Select(a => new Answer { Id = a.Id, Content = a.Content })
                     .ToList()
                 }).ToListAsync(cancellationToken);

            return result;
        }

        public async Task<IEnumerable<int>> GetAvailablePollsIDS(int pollId, CancellationToken cancellationToken)
        {
            var result = await _DbContext.Questions.Where(q => q.PollId == pollId && q.IsActive)
                .Select(q => q.Id)
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}
