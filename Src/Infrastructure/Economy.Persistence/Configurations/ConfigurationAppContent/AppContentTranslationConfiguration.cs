using Economy.Domain.Entites.EntityAppContents.AppContents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Configurations.ConfigurationAppContent
{
    public class AppContentTranslationConfiguration : IEntityTypeConfiguration<AppContentTranslation>
    {
        public void Configure(EntityTypeBuilder<AppContentTranslation> builder)
        {
            builder.ToTable("AppContentTranslations");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title).IsRequired().HasMaxLength(250);
            builder.Property(x => x.Url).IsRequired().HasMaxLength(500);

            builder.HasOne<AppContent>()
                   .WithMany(x => x.Translations)
                   .HasForeignKey(x => x.AppContentId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Seed data
            builder.HasData(
                new AppContentTranslation
                {
                    Id = 1,
                    AppContentId = 1,
                    AppLanguageId = 1,
                    Title = "Oda 1",
                    ShortDescription = "Kısa açıklama 1",
                    Content = "Detaylı içerik 1",
                    IsExternal = false,
                    Url = "/oda-1",
                    MetaTitle = "Oda 1 SEO",
                    MetaDescription = "Oda 1 açıklaması"
                },
                new AppContentTranslation
                {
                    Id = 2,
                    AppContentId = 2,
                    AppLanguageId = 1,
                    Title = "Hakkımızda",
                    ShortDescription = "Kurumsal kısa açıklama",
                    Content = "Şirket hakkında detaylı bilgi",
                    IsExternal = false,
                    Url = "/hakkimizda",
                    MetaTitle = "Hakkımızda SEO",
                    MetaDescription = "Hakkımızda açıklaması"
                }
            );
        }
    }
}
