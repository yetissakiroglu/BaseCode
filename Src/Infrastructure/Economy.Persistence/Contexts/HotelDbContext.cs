using Economy.Domain.Entites.TenantEntity.EntityAppBlocks;
using Economy.Domain.Entites.TenantEntity.EntityAppLanguages;
using Economy.Domain.Entites.TenantEntity.EntityAppMenus;
using Economy.Domain.Entites.TenantEntity.EntityAppPages;
using Economy.Domain.Entites.TenantEntity.EntityAppSettings;
using Economy.Persistence.Tenant.Configurations.ConfigurationAppLanguage;
using Economy.Persistence.Tenant.Configurations.ConfigurationAppSettings;
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
        public DbSet<AppTechnicalSetting> AppTechnicalSettings { get; set; }
        public DbSet<AppSetting> AppSettings { get; set; }
        public DbSet<AppSettingTranslation> AppSettingTranslations { get; set; }
        public DbSet<AppSettingLogo> AppSettingLogos { get; set; }
        public DbSet<AppLanguage> AppLanguages { get; set; }
        public DbSet<AppMenu> AppMenus { get; set; }
        public DbSet<AppMenuTranslation> AppMenuTranslations { get; set; }
        public DbSet<AppPage> AppPages { get; set; }
        public DbSet<AppPageTranslation> AppPageTranslations { get; set; }
        public DbSet<AppPageMedia> AppPageMedias { get; set; }
        public DbSet<AppPageMediaTranslation> AppPageMediaTranslations { get; set; }
        public DbSet<AppBlockGroup> AppBlockGroups => Set<AppBlockGroup>();
        public DbSet<AppBlockGroupTranslation> AppBlockGroupTranslations => Set<AppBlockGroupTranslation>();
        public DbSet<AppBlock> AppBlocks => Set<AppBlock>();
        public DbSet<AppBlockTranslation> AppBlockTranslations => Set<AppBlockTranslation>();
        public DbSet<AppBlockGroupBlock> AppBlockGroupBlocks => Set<AppBlockGroupBlock>();


        

        public DbSet<PageBlock> PageBlocks => Set<PageBlock>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.ApplyConfiguration(new AppLanguage_Configuration()); // ← Burası önemli
            //modelBuilder.ApplyConfiguration(new AppSetting_Configuration()); // ← Burası önemli
            //modelBuilder.ApplyConfiguration(new AppSettingTranslation_Configuration()); // ← Burası önemli
            modelBuilder.ApplyConfiguration(new AppSettingLogoConfiguration()); // ← Burası önemli
            //modelBuilder.ApplyConfiguration(new AppSlideConfiguration());
            //modelBuilder.ApplyConfiguration(new AppSlideTranslationConfiguration());
      

            base.OnModelCreating(modelBuilder);
        }
    }
}