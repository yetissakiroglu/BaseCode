using Economy.Domain.Entites.EntityAppSettings;
using Economy.Persistence.Configurations.ConfigurationAppLanguage;
using Microsoft.EntityFrameworkCore;
namespace Economy.Persistence.Contexts
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<AppSetting> AppSettings { get; set; }
        public DbSet<AppTechnicalSetting> AppTechnicalSettings { get; set; }

        // diğer otel tabloları...

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AppLanguage_Configuration()); // ← Burası önemli

            base.OnModelCreating(builder);
        }
    }
}