using Economy.Domain.BaseEntities;



namespace Economy.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Default (sabit) veritabanındaki değişiklikleri kaydeder.
        /// </summary>
        Task<int> SaveDefaultChangesAsync();
        int SaveDefaultChanges();

        /// <summary>
        /// Otel veritabanındaki değişiklikleri kaydeder.
        /// </summary>
        Task<int> SaveHotelChangesAsync();
        int SaveHotelChanges();

        /// <summary>
        /// Otel bağlantı bilgisini belirler. 
        /// Bu metod çağrılmadan otel işlemleri yapılamaz.
        /// </summary>
        /// <param name="connectionString">Otele özel bağlantı cümlesi</param>
        void SetHotelConnectionString(string connectionString);

        /// <summary>
        /// Belirtilen varlık türü için otel veritabanında çalışan repository döner.
        /// </summary>
        /// <typeparam name="T">Varlık türü</typeparam>
        /// <returns>IEntityRepository</returns>
        IEntityRepository<T, int> EntityRepository<T>() where T : class, ISoftDelete, IHasId<int>;
    }
}


