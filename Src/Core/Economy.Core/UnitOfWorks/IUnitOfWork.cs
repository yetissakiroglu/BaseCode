namespace Economy.Core.UnitOfWorks
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync();
    }
}
