using Economy.Application.Common.Interfaces;
using Economy.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Economy.Infrastructure.Events
{
    public class LoggingHandler : IDomainEventHandler<AppMenuCreatedEvent>
    {
        private readonly ILogger<LoggingHandler> _logger;

        public LoggingHandler(ILogger<LoggingHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(AppMenuCreatedEvent domainEvent)
        {
            _logger.LogInformation("AppMenuCreatedEvent triggered: MenuId={MenuId}, Title={Title}, Slug={Slug}, CreatedBy={CreatedBy}",
                domainEvent.MenuId, domainEvent.Title, domainEvent.Slug, domainEvent.CreatedBy);
            return Task.CompletedTask;
        }
    }
}
