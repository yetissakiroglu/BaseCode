namespace Economy.Domain.Common
{

    public interface IDomainEvent
    {
     
        DateTime OccurredOn { get; }
    }
}
