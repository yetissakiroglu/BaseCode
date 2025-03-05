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
        public async Task AddAsync(T entity)
        {
            await Table.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(T entity)
        {
            entity.IsDeleted = true;
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
        public Task UpdateAsync(T entity)
        {
            Table.Update(entity);
            return Task.CompletedTask;
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>>? filters = null)
        {
            var query = filters == null ? Table : Table.Where(filters);
            return await query.AnyAsync();
        }
        public async Task<bool> AnyNonDeletedAsync(Expression<Func<T, bool>>? filters = null)
        {
            var query = Table.ApplyIsDeletedFalseFilter();
            query = filters == null ? query : query.Where(filters);
            return await query.AnyAsync();
        }
        public async Task<T> GetForEditAsync(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes)
        {
            var query = filters == null ? Table : Table.Where(filters);

            query = includes.Aggregate(query, (current, include) => current.Include(include));

            return await query.FirstOrDefaultAsync();
        }
        public async Task<T> GetForEditNonDeletedAsync(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes)
        {
            var query = Table.ApplyIsDeletedFalseFilter();

            query = filters == null ? query : query.Where(filters);

            query = includes.Aggregate(query, (current, include) => current.Include(include));

            return await query.FirstOrDefaultAsync();
        }
        public async Task<T> GetForReadAsync(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes)
        {
            var query = filters == null ? Table : Table.AsTracking().Where(filters);

            query = includes.Aggregate(query, (current, include) => current.Include(include));

            return await query.FirstOrDefaultAsync();
        }
        public async Task<T> GetForReadNonDeletedAsync(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes)
        {
            var query = Table.AsTracking().ApplyIsDeletedFalseFilter();

            query = filters == null ? query : query.Where(filters);

            query = includes.Aggregate(query, (current, include) => current.Include(include));

            return await query.FirstOrDefaultAsync();
        }
        public async Task<IPagedList<T>> PagedListForReadAsync(Expression<Func<T, bool>>? filters = null, int page = 0, int pageSize = 0)
        {
            var query = filters == null ? await Table.ToListAsync() : await Table.AsTracking().Where(filters).ToListAsync();
            return new PagedList<T>(query, page, pageSize);
        }
        public async Task<IPagedList<T>> PagedListForReadNonDeletedAsync(Expression<Func<T, bool>>? filters = null, int page = 0, int pageSize = 0)
        {
            var query = filters == null ? await Table.ApplyIsDeletedFalseFilter().ToListAsync() : await Table.ApplyIsDeletedFalseFilter().AsTracking().Where(filters).ToListAsync();
            return new PagedList<T>(query, page, pageSize);
        }
        public async Task<IQueryable<T>> WhereForEditAsync(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes)
        {
            var query = filters == null ? Table : Table.Where(filters);
            query = includes.Aggregate(query, (current, include) => current.Include(include));
            return await Task.FromResult(query.AsQueryable());
        }
        public async Task<IQueryable<T>> WhereForEditNonDeletedAsync(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes)
        {
            var query = filters == null ? Table : Table.ApplyIsDeletedFalseFilter().Where(filters);
            query = includes.Aggregate(query, (current, include) => current.Include(include));
            return await Task.FromResult(query.AsQueryable());
        }
        public async Task<IQueryable<T>> WhereForReadAsync(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes)
        {
            var query = filters == null ? Table : Table.Where(filters);
            query = includes.Aggregate(query, (current, include) => current.Include(include));
            return await Task.FromResult(query.AsQueryable());
        }
        public async Task<IQueryable<T>> WhereForReadNonDeletedAsync(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes)
        {
            var query = filters == null ? Table : Table.ApplyIsDeletedFalseFilter().Where(filters);
            query = includes.Aggregate(query, (current, include) => current.Include(include));
            return await Task.FromResult(query.AsQueryable());
        }

       
    }
}
