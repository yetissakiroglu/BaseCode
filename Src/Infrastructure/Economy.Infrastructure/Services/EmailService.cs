using Economy.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace Economy.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        public Task SendEmailAsync(string to, string subject, string body)
        {
            // Simulate sending email
            _logger.LogInformation("Email sent to {To} with subject {Subject}: {Body}", to, subject, body);
            return Task.CompletedTask;
        }
    }
}
