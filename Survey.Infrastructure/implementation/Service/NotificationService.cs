using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Survey.Domain.Entities;
using Survey.Domain.Interfaces.IRepository;
using Survey.Infrastructure.IService;
using Survey.Infrastructure.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.implementation.Service
{
    public class NotificationService(IPollRepo pollRepo, UserManager<ApplicationUser> userManager, Microsoft.AspNetCore.Identity.UI.Services.IEmailSender emailSender) : INotificationService
    {
        private readonly IPollRepo _PollRepo = pollRepo;
        private readonly UserManager<ApplicationUser> _UserManager = userManager;
        private readonly IEmailSender _EmailSender = emailSender;

        public async Task SendNewPollNotification(int? pollId = null)
        {
            IEnumerable<Poll?> polls = [];

            //first check if pollid has value and if not get all polls
            //get all users and loopin through polls and nested loop through users to send maill with each poll to each user

            if (pollId.HasValue)
            {
                var poll = await _PollRepo.NewPoll(pollId.Value);
                polls = [poll];
            }
            else
            {
                polls = await _PollRepo.NewPollsAdded();
            }

            var users = await _UserManager.Users.ToListAsync();

            foreach (var poll in polls)
            {
                foreach (var user in users)
                {
                    var placeholders = new Dictionary<string, string>
                    {
                        {"{{name}}", user.FirstName },
                        {"{{pollTitle}}", poll!.Title },
                        {"{{pollDate}}", poll.EndsAt.ToString("yyyy-MM-dd") },
                        {"{{url}}",$"localhost://44391/polls/start/{poll.Id}"},

                    };
                    var body = EmailBodyBuilder.GenerateBody("PollNotification", placeholders);

                    await _EmailSender.SendEmailAsync(user.Email!, $" 🥳 Survey Site : New Poll - {poll.Id} Added", body);
                }
            }

        }
    }
}
