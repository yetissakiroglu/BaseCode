using Economy.Core.Services.Providers;
using Economy.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Economy.Base.Persistence.Providers
{
    public class MigrationService
    {
        private readonly ILogger<MigrationService> _logger;
        private readonly TenantProvider _tenantProvider;
        private readonly IConfiguration _configuration;

        public MigrationService(ILogger<MigrationService> logger, TenantProvider tenantProvider, IConfiguration configuration)
        {
            _logger = logger;
            _tenantProvider = tenantProvider;
            _configuration = configuration;
        }

        public async Task MigrateTenantAsync(int tenantId)
        {
            try
            {
                var tenantConnectionString = await _tenantProvider.GetConnectionStringAsync();
                var optionsBuilder = new DbContextOptionsBuilder<HotelDbContext>();
                optionsBuilder.UseSqlServer(tenantConnectionString);

                using var context = new HotelDbContext(optionsBuilder.Options);

                // Migration işlemini başlatıyoruz
                await context.Database.MigrateAsync();

                _logger.LogInformation($"Tenant {tenantId} veritabanı migrasyonu başarıyla tamamlandı.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Tenant {tenantId} veritabanı migrasyonu sırasında hata oluştu: {ex.Message}");
            }
        }
        public async Task MigrateAllHotelDatabasesAsync()
        {
            var connectionStrings = await _tenantProvider.GetAllConnectionStringsAsync();

            foreach (var connStr in connectionStrings)
            {
                try
                {
                    var optionsBuilder = new DbContextOptionsBuilder<HotelDbContext>();
                    optionsBuilder.UseSqlServer(connStr);

                    using var context = new HotelDbContext(optionsBuilder.Options);

                    _logger.LogInformation($"Migrating database: {connStr}");
                    await context.Database.MigrateAsync();

                    _logger.LogInformation($"Migration completed for: {connStr}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Migration failed for {connStr}: {ex.Message}");
                }
            }
        }
        public async Task MigrateMasterDbAsync()
        {
            try
            {
                // appsettings.json'dan connection string'i alıyoruz
                var connectionString = _configuration.GetConnectionString("DefaultConnection");

                // DbContext options builder
                var optionsBuilder = new DbContextOptionsBuilder<DefaultDbContext>();
                optionsBuilder.UseSqlServer(connectionString);

                // DbContext'i oluştur
                using var context = new DefaultDbContext(optionsBuilder.Options);

                // Master DB migration işlemi
                await context.Database.MigrateAsync();

                _logger.LogInformation("Master veritabanı migrasyonu başarıyla tamamlandı.");

            }
            catch (Exception ex)
            {
                _logger.LogError($"Master veritabanı migrasyonu sırasında hata oluştu: {ex.Message}");
            }
        }
    }
}
