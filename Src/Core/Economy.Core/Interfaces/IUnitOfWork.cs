using Economy.Domain.BaseEntities;

namespace Economy.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IEntityRepository<T, int> EntityRepository<T>() where T : class, ISoftDelete, IHasId<int>;
        Task<int> SaveChangesAsync();
    }
}
