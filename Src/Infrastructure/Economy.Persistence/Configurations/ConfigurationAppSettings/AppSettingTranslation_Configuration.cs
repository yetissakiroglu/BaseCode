using Economy.Domain.Entites.EntityAppSettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Base.Persistence.Configurations.ConfigurationAppSettings
{
    public class AppSettingTranslation_Configuration : IEntityTypeConfiguration<AppSettingTranslation>
    {
        public void Configure(EntityTypeBuilder<AppSettingTranslation> builder)
        {
            builder.ToTable("AppSettingTranslations");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.SiteTitle).HasMaxLength(200);
            builder.Property(x => x.Description).HasMaxLength(300);
            builder.Property(x => x.MetaTitle).HasMaxLength(200);
            builder.Property(x => x.MetaDescription).HasMaxLength(300);

            builder.HasIndex(x => new { x.AppSettingId, x.AppLanguageId }).IsUnique();

            // Seed verilerini doğru şekilde ekleyin
            builder.HasData(
                new AppSettingTranslation
                {
                    Id = 1,
                    AppSettingId = 1,
                    AppLanguageId = 1, // Türkçe
                    SiteTitle = "Site Başlığı - TR",
                    Description = "Site açıklaması Türkçe",
                    MetaTitle = "Meta Başlık - TR",
                    MetaDescription = "Meta açıklaması Türkçe"
                },
                new AppSettingTranslation
                {
                    Id = 2,
                    AppSettingId = 1,
                    AppLanguageId = 2, // İngilizce
                    SiteTitle = "Site Title - EN",
                    Description = "Site description in English",
                    MetaTitle = "Meta Title - EN",
                    MetaDescription = "Meta description in English"
                }
            );
        }
    }
   
}
