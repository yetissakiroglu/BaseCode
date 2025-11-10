using Economy.Domain.BaseEntities;



namespace Economy.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Default (merkez) DB
        IEntityRepository<T, int> DefaultEntityRepository<T>() where T : class, ISoftDelete, IHasId<int>;
        Task<int> SaveDefaultChangesAsync(CancellationToken ct = default);
        int SaveDefaultChanges();

        // Hotel (tenant) DB
        void SetHotelConnectionString(string connectionString);
        IEntityRepository<T, int> HotelEntityRepository<T>() where T : class, ISoftDelete, IHasId<int>;
        Task<int> SaveHotelChangesAsync(CancellationToken ct = default);
        int SaveHotelChanges();

        // İsteğe bağlı: Hotel DB için transaction sarmalayıcı
        Task ExecuteHotelTxAsync(Func<Task> work, CancellationToken ct = default);
    }
}


