using Economy.Core.PagingModels;
using System.Linq.Expressions;

namespace Economy.Core.Interfaces
{
    public interface IEntityRepository<T, TId> where T : class
    {
        T? GetForRead(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes);
        T? GetForEdit(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes);
        List<T> WhereForRead(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes);
        List<T> WhereForEdit(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes);
        T? GetForReadFunc(Expression<Func<T, bool>>? filters = null, params Func<IQueryable<T>, IQueryable<T>>[] includes);
        T? GetForEditFunc(Expression<Func<T, bool>>? filters = null, params Func<IQueryable<T>, IQueryable<T>>[] includes);

        List<T> WhereForReadFunc(Expression<Func<T, bool>>? filters = null, params Func<IQueryable<T>, IQueryable<T>>[] includes);
        IPagedList<T> PagedListForRead(Expression<Func<T, bool>>? filters = null, int page = 0, int pageSize = 0);
        bool Any(Expression<Func<T, bool>>? filters = null);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(int Id);
    }
}
