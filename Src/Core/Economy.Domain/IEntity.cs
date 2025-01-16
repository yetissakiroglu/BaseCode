namespace Economy.Domain
{
    public abstract class IEntity<T>
    {
        public T Id { get; set; } = default!;
    }
}
