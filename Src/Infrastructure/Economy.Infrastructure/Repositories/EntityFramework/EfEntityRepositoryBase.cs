using Economy.Core.Interfaces;
using Economy.Core.PagingModels;
using Economy.Domain.BaseEntities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Economy.Persistence.Repositories.AppBase.EntityFramework
{
    public class EfEntityRepositoryBase<T, TId> : IEntityRepository<T, TId> where T : class, ISoftDelete, IHasId<TId>
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _entities;

        public EfEntityRepositoryBase(DbContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }

        public void Add(T entity)
        {
            _entities.Add(entity);
        }
        public void Delete(T entity)
        {
            entity.IsDeleted = true;
            _context.Update(entity);
        }
        public void Update(T entity)
        {
            _entities.Update(entity);
        }

        public bool Any(Expression<Func<T, bool>>? filters = null)
        {
            var query = filters == null ? _entities : _entities.Where(filters);
            return query.Any();
        }
   
        public T? GetForEdit(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes)
        {
            var query = filters == null ? _entities : _entities.Where(filters);

            query = includes.Aggregate(query, (current, include) => current.Include(include));

            return  query.FirstOrDefault();
        }
     
        public T? GetForRead(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes)
        {
            var query = filters == null ? _entities : _entities.AsNoTracking().Where(filters);

            query = includes.Aggregate(query, (current, include) => current.Include(include));

            return query.FirstOrDefault();
        }
     
        public IPagedList<T> PagedListForRead(Expression<Func<T, bool>>? filters = null, int page = 0, int pageSize = 0)
        {
            var query = filters == null ? _entities.ToList() : _entities.AsTracking().Where(filters).ToList();
            return new PagedList<T>(query, page, pageSize);
        }
      
        public List<T> WhereForEdit(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes)
        {
            var query = filters == null ? _entities : _entities.Where(filters);
            query = includes.Aggregate(query, (current, include) => current.Include(include));
            return query.ToList();
        }
   
        public List<T> WhereForRead(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes)
        {
            var query = filters == null ? _entities.AsNoTracking() : _entities.Where(filters).AsNoTracking();
            query = includes.Aggregate(query, (current, include) => current.Include(include));
            return query.ToList();
        }
       
        public T? GetForReadFunc(Expression<Func<T, bool>>? filters = null, params Func<IQueryable<T>, IQueryable<T>>[] includes)
        {
            var query = _entities.AsNoTracking(); // Performans için AsNoTracking kullan

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
            var query = _entities.AsNoTracking(); // Performans için AsNoTracking kullan

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
