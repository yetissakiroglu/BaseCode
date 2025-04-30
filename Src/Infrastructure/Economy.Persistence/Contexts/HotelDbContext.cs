using Economy.Domain.Entites.EntityAppSettings;
using Microsoft.EntityFrameworkCore;

namespace Economy.Persistence.Contexts
{
  
    public class HotelDbContext(DbContextOptions<HotelDbContext> options) : DbContext(options)
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<AppSetting> AppSettings { get; set; }
        // diğer otel tabloları...
  
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }

  
}
