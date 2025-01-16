using Economy.Application.Infrastructure.Paging;
using Economy.Application.Repositories;
using Economy.Domain.BaseEntities;
using Economy.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Economy.Persistence.Repositories.AppBase.EntityFramework
{
    public class EfEntityRepositoryBase<T, TId>(AppDbContext _context) : IEntityRepository<T, TId> where T : class, ISoftDelete, IHasId<TId>, new()
    {


        public DbSet<T> Table => _context.Set<T>();

        /* --- Yeni Repository --- */

        public async Task<IQueryable<T>> GetAllIncludingDeletedAsync()
        {
            return await Task.FromResult(Table.AsNoTracking().AsQueryable());
        }

        public async Task<IQueryable<T>> GetAllOnlyDeletedAsync()
        {
            return await Task.FromResult(Table.AsNoTracking().Where(x => x.IsDeleted == true).AsQueryable());
        }

        public async Task<IQueryable<T>> GetAllOnlyNonDeletedAsync()
        {
            return await Task.FromResult(Table.AsNoTracking()
                                   .ApplyIsDeletedFalseFilter(isApplyFilter: true)
                                   .AsQueryable());

        }

        public async Task<IQueryable<T>> WhereIncludingDeletedAsync(Expression<Func<T, bool>> expression)
        {
            return await Task.FromResult(Table.AsNoTracking()
                .ApplyIsDeletedFalseFilter(isApplyFilter: false)
                .Where(expression).AsQueryable());
        }

        public async Task<IQueryable<T>> WhereOnlyDeletedAsync(Expression<Func<T, bool>> expression)
        {
            return await Task.FromResult(Table.AsNoTracking()
                                          .Where(x => x.IsDeleted == true)
                                          .Where(expression)
                                          .AsQueryable());
        }

        public async Task<IQueryable<T>> WhereOnlyNonDeletedAsync(Expression<Func<T, bool>> expression)
        {
            return await Task.FromResult(Table.AsNoTracking()
                                   .ApplyIsDeletedFalseFilter(isApplyFilter: true)
                                  .Where(expression)
                                  .AsQueryable());
        }

        public async Task<IPagedList<T>> GetAllIncludingDeletedPagedAsync(int page = 0, int pageSize = 0)
        {
            var query = await Table
                   .ApplyIsDeletedFalseFilter(isApplyFilter: false)
                   .AsNoTracking()
                   .ToListAsync();

            return new PagedList<T>(query, page, pageSize);

        }

        public async Task<IPagedList<T>> GetAllOnlyDeletedPagedAsync(int page = 0, int pageSize = 0)
        {
            var query = await Table
                .Where(x => x.IsDeleted == true)
               .AsNoTracking()
               .ToListAsync();

            return new PagedList<T>(query, page, pageSize);
        }

        public async Task<IPagedList<T>> GetAllOnlyNonDeletedPagedAsync(int page = 0, int pageSize = 0)
        {
            var query = await Table
                       .ApplyIsDeletedFalseFilter(isApplyFilter: true)
                       .AsNoTracking()
                       .ToListAsync();

            return new PagedList<T>(query, page, pageSize);
        }

        public async Task AddAsync(T entity)
        {
            await Table.AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            await Task.FromResult(Table.Update(entity));
        }

        public async Task DeleteAsync(T entity)
        {
            await Task.FromResult(Table.Remove(entity));
        }
    }
}
