using Economy.Base.Persistence.Configurations.ConfigurationAppSettings;
using Economy.Domain.Entites.EntityAppSettings;
using Economy.Domain.Entites.EntitySlides;
using Economy.Persistence.Configurations.ConfigurationAppLanguage;
using Economy.Persistence.Configurations.ConfigurationAppSlide;
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
        public DbSet<AppSettingLogo> AppSettingLogos { get; set; }      
        public DbSet<AppTechnicalSetting> AppTechnicalSettings { get; set; }
        public DbSet<AppSettingReservationLink> AppSettingReservationLinks { get; set; }
        public DbSet<AppSettingReservationNumber> AppSettingReservationNumbers { get; set; }
        public DbSet<AppSettingWhatsappLine> AppSettingWhatsappLines { get; set; }
        public DbSet<AppSlide> AppSlides { get; set; }


        // diğer otel tabloları...

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AppLanguage_Configuration()); // ← Burası önemli
            modelBuilder.ApplyConfiguration(new AppSetting_Configuration()); // ← Burası önemli
            modelBuilder.ApplyConfiguration(new AppSettingTranslation_Configuration()); // ← Burası önemli
            modelBuilder.ApplyConfiguration(new AppSettingLogoConfiguration()); // ← Burası önemli
            modelBuilder.ApplyConfiguration(new AppSettingReservationNumberConfiguration());
            modelBuilder.ApplyConfiguration(new AppSettingReservationLinkConfiguration());
            modelBuilder.ApplyConfiguration(new AppSettingWhatsappLineConfiguration());
            modelBuilder.ApplyConfiguration(new AppSlideConfiguration());
            modelBuilder.ApplyConfiguration(new AppSlideTranslationConfiguration());


            base.OnModelCreating(modelBuilder);
        }
    }
}