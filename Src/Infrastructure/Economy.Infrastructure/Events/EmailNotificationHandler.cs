using Economy.Application.Common.Interfaces;
using Economy.Domain.Events;
using Microsoft.Extensions.Logging;


namespace Economy.Infrastructure.Events
{
    public class EmailNotificationHandler : IDomainEventHandler<AppMenuCreatedEvent>
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<EmailNotificationHandler> _logger;

        public EmailNotificationHandler(IEmailService emailService, ILogger<EmailNotificationHandler> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        public async Task HandleAsync(AppMenuCreatedEvent domainEvent)
        {
            var subject = "New Menu Created";
            var message = $"A new menu '{domainEvent.Title}' has been created with slug '{domainEvent.Slug}' by {domainEvent.CreatedBy}.";
            await _emailService.SendEmailAsync("admin@economy.com", subject, message);
            _logger.LogInformation("Email sent for AppMenuCreatedEvent: {Title}", domainEvent.Title);
        }
    }
}
