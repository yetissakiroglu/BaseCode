using Economy.Domain.Common;

namespace Economy.Application.Common.Interfaces
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(IDomainEvent domainEvent);
    }
}
