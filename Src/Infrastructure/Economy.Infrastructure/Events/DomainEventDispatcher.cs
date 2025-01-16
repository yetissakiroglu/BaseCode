using Economy.Application.Common.Interfaces;
using Economy.Domain.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Economy.Infrastructure.Events
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public DomainEventDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task DispatchAsync(IDomainEvent domainEvent)
        {
            var handlers = _serviceProvider.GetServices<IDomainEventHandler<IDomainEvent>>();
            foreach (var handler in handlers)
            {
                if (handler.GetType().GetInterfaces()[0].GenericTypeArguments[0] == domainEvent.GetType())
                {
                    await ((dynamic)handler).HandleAsync((dynamic)domainEvent);
                }
            }
        }
    }
}
