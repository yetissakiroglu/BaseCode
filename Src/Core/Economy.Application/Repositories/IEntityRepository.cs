using Economy.Application.Infrastructure.Paging;
using System.Linq.Expressions;

namespace Economy.Application.Repositories
{
    public interface IEntityRepository<T, TId> : IRepository<T> where T : class, new()
	{
		Task<IQueryable<T>> GetAllIncludingDeletedAsync();
		Task<IQueryable<T>> GetAllOnlyDeletedAsync();
		Task<IQueryable<T>> GetAllOnlyNonDeletedAsync();

		Task<IQueryable<T>> WhereIncludingDeletedAsync(Expression<Func<T, bool>> expression);
		Task<IQueryable<T>> WhereOnlyDeletedAsync(Expression<Func<T, bool>> expression);
		Task<IQueryable<T>> WhereOnlyNonDeletedAsync(Expression<Func<T, bool>> expression);

		Task<IPagedList<T>> GetAllIncludingDeletedPagedAsync(int page = 0, int pageSize = 0);
		Task<IPagedList<T>> GetAllOnlyDeletedPagedAsync(int page = 0, int pageSize = 0);
		Task<IPagedList<T>> GetAllOnlyNonDeletedPagedAsync(int page = 0, int pageSize = 0);




		Task AddAsync(T entity);
		Task UpdateAsync(T entity);
		Task DeleteAsync(T entity);
    }
}
