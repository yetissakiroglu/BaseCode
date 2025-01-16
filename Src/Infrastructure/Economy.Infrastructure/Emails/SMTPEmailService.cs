using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity.UI.Services;
using MailKit.Net.Smtp;
using MimeKit;

namespace Economy.Infrastructure.Emails
{
    public class SMTPEmailService : IEmailSender
    {
        private readonly SMTPConfiguration _smtpConfiguration;

        public SMTPEmailService(IOptions<SMTPConfiguration> smtpConfiguration)
        {
            _smtpConfiguration = smtpConfiguration.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_smtpConfiguration.FromName, _smtpConfiguration.FromAddress));
            message.To.Add(new MailboxAddress(email, email));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlMessage
            };

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpConfiguration.Host, _smtpConfiguration.Port, true);
                await client.AuthenticateAsync(_smtpConfiguration.UserName, _smtpConfiguration.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
