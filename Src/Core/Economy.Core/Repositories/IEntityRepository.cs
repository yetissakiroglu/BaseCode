using Economy.Core.PagingModels;
using System.Linq.Expressions;

namespace Economy.Core.Repositories
{
    public interface IEntityRepository<T, TId> : IRepository<T> where T : class, new()
	{
        T? GetForRead(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes);
        T? GetForReadNonDeleted(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes);

        T? GetForEdit(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes);
        T? GetForEditNonDeleted(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes);

        List<T> WhereForRead(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes);
        List<T> WhereForReadNonDeleted(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes);

        List<T> WhereForEdit(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes);
        List<T> WhereForEditNonDeleted(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes);

        IPagedList<T> PagedListForRead(Expression<Func<T, bool>>? filters = null, int page = 0, int pageSize = 0);
        IPagedList<T> PagedListForReadNonDeleted(Expression<Func<T, bool>>? filters = null, int page = 0, int pageSize = 0);

        bool Any(Expression<Func<T, bool>>? filters = null);
        bool AnyNonDeleted(Expression<Func<T, bool>>? filters = null);

        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);

        T? GetForReadFunc(Expression<Func<T, bool>>? filters = null, params Func<IQueryable<T>, IQueryable<T>>[] includes);
        List<T> WhereForReadFunc(Expression<Func<T, bool>>? filters = null, params Func<IQueryable<T>, IQueryable<T>>[] includes);

    }
}
