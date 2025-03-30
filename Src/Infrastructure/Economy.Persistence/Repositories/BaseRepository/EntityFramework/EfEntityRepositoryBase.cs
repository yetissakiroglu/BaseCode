using Economy.Core.PagingModels;
using Economy.Core.Repositories;
using Economy.Domain.BaseEntities;
using Economy.Persistence.Contexts;
using Economy.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Economy.Persistence.Repositories.AppBase.EntityFramework
{
    public class EfEntityRepositoryBase<T, TId>(AppDbContext _context) : IEntityRepository<T, TId> where T : class, ISoftDelete, IHasId<TId>, new()
    {
        public DbSet<T> Table => _context.Set<T>();
        public void Add(T entity)
        {
            Table.Add(entity);
        }
        public void Delete(T entity)
        {
            entity.IsDeleted = true;
            _context.Update(entity);
        }
        public void Update(T entity)
        {
            Table.Update(entity);
        }

        public bool Any(Expression<Func<T, bool>>? filters = null)
        {
            var query = filters == null ? Table : Table.Where(filters);
            return query.Any();
        }
        public bool AnyNonDeleted(Expression<Func<T, bool>>? filters = null)
        {
            var query = Table.ApplyIsDeletedFalseFilter();
            query = filters == null ? query : query.Where(filters);
            return query.Any();
        }
        public T? GetForEdit(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes)
        {
            var query = filters == null ? Table : Table.Where(filters);

            query = includes.Aggregate(query, (current, include) => current.Include(include));

            return  query.FirstOrDefault();
        }
        public T? GetForEditNonDeleted(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes)
        {
            var query = Table.ApplyIsDeletedFalseFilter();
            query = filters == null ? query : query.Where(filters);
            query = includes.Aggregate(query, (current, include) => current.Include(include));
            return query.FirstOrDefault();
        }
        public T? GetForRead(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes)
        {
            var query = filters == null ? Table : Table.AsNoTracking().Where(filters);

            query = includes.Aggregate(query, (current, include) => current.Include(include));

            return query.FirstOrDefault();
        }
        public T? GetForReadNonDeleted(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes)
        {
            var query = Table.AsTracking().ApplyIsDeletedFalseFilter();

            query = filters == null ? query : query.Where(filters);

            query = includes.Aggregate(query, (current, include) => current.Include(include));

            return query.FirstOrDefault();
        }
        public IPagedList<T> PagedListForRead(Expression<Func<T, bool>>? filters = null, int page = 0, int pageSize = 0)
        {
            var query = filters == null ? Table.ToList() : Table.AsTracking().Where(filters).ToList();
            return new PagedList<T>(query, page, pageSize);
        }
        public IPagedList<T> PagedListForReadNonDeleted(Expression<Func<T, bool>>? filters = null, int page = 0, int pageSize = 0)
        {
            var query = filters == null ? Table.ApplyIsDeletedFalseFilter().ToList() : Table.ApplyIsDeletedFalseFilter().AsTracking().Where(filters).ToList();
            return new PagedList<T>(query, page, pageSize);
        }
        public List<T> WhereForEdit(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes)
        {
            var query = filters == null ? Table : Table.Where(filters);
            query = includes.Aggregate(query, (current, include) => current.Include(include));
            return query.ToList();
        }
        public List<T>WhereForEditNonDeleted(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes)
        {
            var query = filters == null ? Table : Table.ApplyIsDeletedFalseFilter().Where(filters);
            query = includes.Aggregate(query, (current, include) => current.Include(include));
            return query.ToList();
        }
        public List<T> WhereForRead(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes)
        {
            var query = filters == null ? Table.AsNoTracking() : Table.Where(filters).AsNoTracking();
            query = includes.Aggregate(query, (current, include) => current.Include(include));
            return query.ToList();
        }
        public List<T> WhereForReadNonDeleted(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes)
        {
            var query = filters == null ? Table : Table.ApplyIsDeletedFalseFilter().Where(filters);
            query = includes.Aggregate(query, (current, include) => current.Include(include));
            return query.ToList();
        }
        public T? GetForReadFunc(Expression<Func<T, bool>>? filters = null, params Func<IQueryable<T>, IQueryable<T>>[] includes)
        {
            var query = Table.AsNoTracking(); // Performans için AsNoTracking kullan

            if (filters != null)
            {
                query = query.Where(filters);
            }

            // Include işlemlerini uygula (Include + ThenInclude desteği)
            foreach (var include in includes)
            {
                query = include(query);
            }

            return query.FirstOrDefault();
        }
        public List<T> WhereForReadFunc(Expression<Func<T, bool>>? filters = null,params Func<IQueryable<T>, IQueryable<T>>[] includes)
        {
            var query = Table.AsNoTracking(); // Performans için AsNoTracking kullan

            if (filters != null)
            {
                query = query.Where(filters);
            }

            // Include işlemlerini uygula (Include + ThenInclude desteği)
            foreach (var include in includes)
            {
                query = include(query);
            }

            return query.ToList();
        }
    }
}
