using Economy.Core.ContextFactory;
using Economy.Core.Interfaces;
using Economy.Domain.BaseEntities;
using Economy.Persistence.Contexts;
using Economy.Persistence.Repositories.AppBase.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Economy.Persistence.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DefaultDbContext _defaultDbContext;
        private readonly IHotelDbContextFactory _hotelDbContextFactory;

        private DbContext _hotelDbContext;
        private string _activeHotelConnectionString;
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(DefaultDbContext defaultDbContext, IHotelDbContextFactory hotelDbContextFactory)
        {
            _defaultDbContext = defaultDbContext ?? throw new ArgumentNullException(nameof(defaultDbContext));
            _hotelDbContextFactory = hotelDbContextFactory ?? throw new ArgumentNullException(nameof(hotelDbContextFactory));
        }

        public void SetHotelConnectionString(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));

            if (_hotelDbContext != null && _activeHotelConnectionString != connectionString)
            {
                throw new InvalidOperationException("This UnitOfWork instance is already bound to a different hotel connection.");
            }

            if (_hotelDbContext == null)
            {
                _hotelDbContext = _hotelDbContextFactory.CreateDbContext(connectionString);
                _activeHotelConnectionString = connectionString;
            }
        }

        public IEntityRepository<T, int> EntityRepository<T>() where T : class, ISoftDelete, IHasId<int>
        {
            if (_hotelDbContext == null)
                throw new InvalidOperationException("HotelDbContext has not been initialized. Call SetHotelConnectionString first.");

            if (_repositories.TryGetValue(typeof(T), out var existingRepo))
            {
                return (IEntityRepository<T, int>)existingRepo;
            }

            var newRepo = new EfEntityRepositoryBase<T>(_hotelDbContext);
            _repositories[typeof(T)] = newRepo;
            return newRepo;
        }

        public async Task<int> SaveDefaultChangesAsync()
        {
            return await _defaultDbContext.SaveChangesAsync();
        }

        public int SaveDefaultChanges()
        {
            return _defaultDbContext.SaveChanges();
        }

        public async Task<int> SaveHotelChangesAsync()
        {
            if (_hotelDbContext == null)
                throw new InvalidOperationException("HotelDbContext has not been initialized.");

            return await _hotelDbContext.SaveChangesAsync();
        }

        public int SaveHotelChanges()
        {
            if (_hotelDbContext == null)
                throw new InvalidOperationException("HotelDbContext has not been initialized.");

            return _hotelDbContext.SaveChanges();
        }

        public void Dispose()
        {
            _defaultDbContext?.Dispose();
            _hotelDbContext?.Dispose();
            _repositories.Clear();
        }
    }
}
