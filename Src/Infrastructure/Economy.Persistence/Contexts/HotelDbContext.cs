using Economy.Domain.Entites.TenantEntity.EntityAppBlocks;
using Economy.Domain.Entites.TenantEntity.EntityAppContents;
using Economy.Domain.Entites.TenantEntity.EntityAppLanguages;
using Economy.Domain.Entites.TenantEntity.EntityAppMenus;
using Economy.Domain.Entites.TenantEntity.EntityAppPages;
using Economy.Domain.Entites.TenantEntity.EntityAppSettings;
using Economy.Domain.Entites.TenantEntity.EntityAppSlides;
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

        /*-------------------------*/
        public DbSet<AppPage> AppPages => Set<AppPage>();
        public DbSet<AppPageTranslation> AppPageTranslations => Set<AppPageTranslation>();
        public DbSet<AppPageBlock> AppPageBlocks => Set<AppPageBlock>();
        public DbSet<AppPageBlockTranslation> AppPageBlockTranslations => Set<AppPageBlockTranslation>();
    
        /*-------------------------*/










        public DbSet<BlockGroup> BlockGroups => Set<BlockGroup>();
        public DbSet<BlockItem> BlockItems => Set<BlockItem>();
        public DbSet<BlockItemImage> BlockItemImages => Set<BlockItemImage>();
        public DbSet<PageBlock> PageBlocks => Set<PageBlock>();

        


        //-------------------------


        public DbSet<AppMenu> AppMenus { get; set; }
        public DbSet<AppMenuTranslation> AppMenuTranslations { get; set; }
        public DbSet<AppSlide> AppSlides { get; set; }
        public DbSet<AppSlideTranslation> AppSlideTranslations { get; set; }


        // === İçerik (çekirdek) ===
        public DbSet<ContentItem> ContentItem { get; set; }
        public DbSet<ContentItemTranslation> ContentItemTranslation { get; set; }

        // === Medya (galeri) ===
        public DbSet<ContentMedia> ContentMedia { get; set; }
        public DbSet<ContentMediaTranslation> ContentMediaTranslation { get; set; }




     
        // diğer otel tabloları...

        protected override void OnModelCreating(ModelBuilder b)
        {


            b.Entity<AppPage>().HasIndex(x => new { x.Slug }).IsUnique();

            b.Entity<AppPageTranslation>()
                .HasIndex(x => new { x.AppPageId, x.AppLanguageId }).IsUnique();

            b.Entity<PageBlock>().HasIndex(x => new { x.PageId, x.SortOrder });

            b.Entity<AppBlockLibrary>()
                .HasIndex(x => new { x.Name });

            b.Entity<AppBlockLibraryTranslation>()
                .HasIndex(x => new { x.AppBlockLibraryId, x.AppLanguageId }).IsUnique();

            b.Entity<AppPageBlock>().Property(x => x.SharedJson).HasColumnType("nvarchar(max)");
            b.Entity<AppPageBlockTranslation>().Property(x => x.LocalizedJson).HasColumnType("nvarchar(max)");
            b.Entity<AppBlockLibrary>().Property(x => x.SharedJson).HasColumnType("nvarchar(max)");
            b.Entity<AppBlockLibraryTranslation>().Property(x => x.LocalizedJson).HasColumnType("nvarchar(max)");


            b.ApplyConfiguration(new AppLanguage_Configuration()); // ← Burası önemli
            //modelBuilder.ApplyConfiguration(new AppSetting_Configuration()); // ← Burası önemli
            //modelBuilder.ApplyConfiguration(new AppSettingTranslation_Configuration()); // ← Burası önemli
            b.ApplyConfiguration(new AppSettingLogoConfiguration()); // ← Burası önemli
            //modelBuilder.ApplyConfiguration(new AppSlideConfiguration());
            //modelBuilder.ApplyConfiguration(new AppSlideTranslationConfiguration());
      

            base.OnModelCreating(b);
        }
    }
}