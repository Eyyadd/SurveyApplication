using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using Survey.Infrastructure.Setting;

namespace Survey.Infrastructure.implementation.Service
{
    public class EmailService(IOptions<MailSetting> mailSetting) : IEmailSender
    {
        private readonly MailSetting _MailSetting = mailSetting.Value;

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_MailSetting.Mail),
                Subject = subject
            };

            message.To.Add(MailboxAddress.Parse(email));

            var builder = new BodyBuilder
            {
                HtmlBody = htmlMessage
            };

            message.Body = builder.ToMessageBody();

            using var smpt = new SmtpClient();
            smpt.Connect(_MailSetting.Host, _MailSetting.Port, SecureSocketOptions.StartTls);
            smpt.Authenticate(_MailSetting.Mail, _MailSetting.Password);
            await smpt.SendAsync(message);
            smpt.Disconnect(true);
        }
    }
}
