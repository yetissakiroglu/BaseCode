namespace Economy.Domain.BaseEntities
{
    public abstract class IEntity<T>
    {
        public T Id { get; set; } = default!;
    }
}
