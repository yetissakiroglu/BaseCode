using Economy.Core.ContextFactory;
using Economy.Core.Interfaces;
using Economy.Domain.BaseEntities;
using Economy.Persistence.Contexts;
using Economy.Persistence.Repositories.AppBase.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Economy.Persistence.Repositories.UnitOfWork
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly DefaultDbContext _defaultDb;
        private readonly IHotelDbContextFactory _hotelFactory;

        // Dinamik (tenant) context ve aktif bağlantı
        private DbContext? _hotelDb;
        private string? _activeHotelConn;

        // Repository cache: (contextInstance, entityType) -> repo
        // Scope içinde tek thread olduğundan Dictionary yeterli.
        private readonly Dictionary<(object CtxKey, Type Entity), object> _repos = new();

        public UnitOfWork(DefaultDbContext defaultDbContext, IHotelDbContextFactory hotelDbContextFactory)
        {
            _defaultDb = defaultDbContext ?? throw new ArgumentNullException(nameof(defaultDbContext));
            _hotelFactory = hotelDbContextFactory ?? throw new ArgumentNullException(nameof(hotelDbContextFactory));
        }

        // ---------- Default (merkez) DB ----------

        public IEntityRepository<T, int> DefaultEntityRepository<T>()
            where T : class, ISoftDelete, IHasId<int>
            => GetRepository<T>(_defaultDb);

        public Task<int> SaveDefaultChangesAsync(CancellationToken ct = default)
            => _defaultDb.SaveChangesAsync(ct);

        public int SaveDefaultChanges()
            => _defaultDb.SaveChanges();

        // ---------- Hotel (tenant) DB ----------

        /// <summary>
        /// Bu istek için kullanılacak otel bağlantısını belirler.
        /// Aynı conn ise no-op; farklı conn ise mevcut HotelDbContext dispose edilip yenisi oluşturulur.
        /// </summary>
        public void SetHotelConnectionString(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));

            // Aynı bağlantıysa hiçbir şey yapma
            if (_hotelDb != null && string.Equals(_activeHotelConn, connectionString, StringComparison.Ordinal))
                return;

            // Farklı bağlantıysa eski context'i kapat ve o context'e ait repo cache'ini temizle
            if (_hotelDb != null)
            {
                var oldCtx = _hotelDb;
                _hotelDb.Dispose();
                _hotelDb = null;

                // Eski context’e bağlı tüm repo'ları temizle
                foreach (var key in _repos.Keys.Where(k => ReferenceEquals(k.CtxKey, oldCtx)).ToList())
                    _repos.Remove(key);
            }

            // Yeni context'i factory ile üret
            _hotelDb = _hotelFactory.CreateDbContext(connectionString);
            _activeHotelConn = connectionString;
        }

        public IEntityRepository<T, int> HotelEntityRepository<T>()
            where T : class, ISoftDelete, IHasId<int>
        {
            if (_hotelDb is null)
                throw new InvalidOperationException("HotelDbContext has not been initialized. Call SetHotelConnectionString first.");
            return GetRepository<T>(_hotelDb);
        }

        public Task<int> SaveHotelChangesAsync(CancellationToken ct = default)
            => _hotelDb is not null
               ? _hotelDb.SaveChangesAsync(ct)
               : throw new InvalidOperationException("HotelDbContext has not been initialized.");

        public int SaveHotelChanges()
            => _hotelDb is not null
               ? _hotelDb.SaveChanges()
               : throw new InvalidOperationException("HotelDbContext has not been initialized.");

        public async Task ExecuteHotelTxAsync(Func<Task> work, CancellationToken ct = default)
        {
            if (_hotelDb is null) throw new InvalidOperationException("HotelDbContext has not been initialized.");

            await using var tx = await _hotelDb.Database.BeginTransactionAsync(ct);
            try
            {
                await work();
                await tx.CommitAsync(ct);
            }
            catch
            {
                await tx.RollbackAsync(ct);
                throw;
            }
        }

        // ---------- Ortak / Yardımcı ----------

        private IEntityRepository<T, int> GetRepository<T>(DbContext ctx)
            where T : class, ISoftDelete, IHasId<int>
        {
            var key = ((object)ctx, typeof(T));
            if (_repos.TryGetValue(key, out var existing))
                return (IEntityRepository<T, int>)existing;

            var repo = new EfEntityRepositoryBase<T>(ctx);
            _repos[key] = repo;
            return repo;
        }

        public void Dispose()
        {
            // DefaultDbContext'i DI dispose eder; burada ELLE dispose etmeyiz.
            _hotelDb?.Dispose();
            _repos.Clear();
        }
    }
}
