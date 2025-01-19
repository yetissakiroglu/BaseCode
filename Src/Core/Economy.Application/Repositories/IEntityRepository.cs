using Economy.Application.Infrastructure.Paging;
using System.Linq.Expressions;

namespace Economy.Application.Repositories
{
    public interface IEntityRepository<T, TId> : IRepository<T> where T : class, new()
	{
        Task<T> GetForReadAsync(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes);
        Task<T> GetForReadNonDeletedAsync(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes);

        Task<T> GetForEditAsync(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes);
        Task<T> GetForEditNonDeletedAsync(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes);

        Task<IQueryable<T>> WhereForReadAsync(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes);
        Task<IQueryable<T>> WhereForReadNonDeletedAsync(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes);

        Task<IQueryable<T>> WhereForEditAsync(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes);
        Task<IQueryable<T>> WhereForEditNonDeletedAsync(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes);

        Task<IPagedList<T>> PagedListForReadAsync(Expression<Func<T, bool>>? filters = null, int page = 0, int pageSize = 0);
        Task<IPagedList<T>> PagedListForReadNonDeletedAsync(Expression<Func<T, bool>>? filters = null, int page = 0, int pageSize = 0);

        Task<bool> AnyAsync(Expression<Func<T, bool>>? filters = null);
        Task<bool> AnyNonDeletedAsync(Expression<Func<T, bool>>? filters = null);

        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
     
    }
}
