using Economy.Core.ContextFactory;
using Economy.Core.Interfaces;
using Economy.Domain.BaseEntities;
using Economy.Persistence.Contexts;
using Economy.Persistence.Repositories.AppBase.EntityFramework;

namespace Economy.Persistence.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DefaultDbContext _defaultDbContext;  // Default DbContext
        private readonly IHotelDbContextFactory _hotelDbContextFactory;
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(DefaultDbContext defaultDbContext, IHotelDbContextFactory hotelDbContextFactory)
        {
            _defaultDbContext = defaultDbContext ?? throw new ArgumentNullException(nameof(defaultDbContext));
            _hotelDbContextFactory = hotelDbContextFactory ?? throw new ArgumentNullException(nameof(hotelDbContextFactory));
        }

        // DefaultDbContext kullanarak repository alır
        public IEntityRepository<T, int> EntityRepository<T>(string connectionString) where T : class, ISoftDelete, IHasId<int>
        {
            // Kullanıcıya özel veritabanı bağlantısı ile repository oluşturulur
            var hotelDbContext = _hotelDbContextFactory.CreateDbContext(connectionString);

            if (_repositories.TryGetValue(typeof(T), out var repo))
                return (IEntityRepository<T, int>)repo;

            var newRepo = new EfEntityRepositoryBase<T>(hotelDbContext);
            _repositories.Add(typeof(T), newRepo);
            return newRepo;
        }

        // DefaultDbContext'teki değişiklikleri kaydeder
        public async Task<int> SaveChangesAsync()
        {
            var defaultDbChanges = await _defaultDbContext.SaveChangesAsync();
            return defaultDbChanges;
        }

        public void Dispose()
        {
            _defaultDbContext.Dispose();
        }
    }
}
