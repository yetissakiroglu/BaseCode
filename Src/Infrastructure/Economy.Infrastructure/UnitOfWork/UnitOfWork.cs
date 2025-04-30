using Economy.Core.Interfaces;
using Economy.Domain.BaseEntities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Economy.Persistence.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEntityRepository<T, int> EntityRepository<T>() where T : class, ISoftDelete, IHasId<int>
        {
            
            if (_repositories.TryGetValue(typeof(T), out var repo))
                return (IEntityRepository<T, int>)repo;

            var newRepo = new Economy.Persistence.Repositories.AppBase.EntityFramework.EfEntityRepositoryBase<T,int>(_context);
            _repositories.Add(typeof(T), newRepo);
            return newRepo;
        }

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();

      
    }
}
