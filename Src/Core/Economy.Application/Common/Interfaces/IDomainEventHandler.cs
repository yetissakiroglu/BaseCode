using Economy.Domain.Common;

namespace Economy.Application.Common.Interfaces
{
    public interface IDomainEventHandler<in TEvent> where TEvent : IDomainEvent
    {
        Task HandleAsync(TEvent domainEvent);
    }
}
