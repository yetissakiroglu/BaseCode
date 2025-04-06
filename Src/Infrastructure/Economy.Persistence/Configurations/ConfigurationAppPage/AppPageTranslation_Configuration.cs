using Economy.Domain.Entites.EntityAppPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Configurations.ConfigurationAppPage
{
    public class AppPageTranslation_Configuration : IEntityTypeConfiguration<AppPageTranslation>
    {
        public void Configure(EntityTypeBuilder<AppPageTranslation> builder)
        {
            builder.ToTable("AppPageTranslations");

            builder.HasKey(x => x.Id);
            // Seed Data
            builder.HasData(
               new AppPageTranslation
               {
                   Id = 1,
                   AppPageId = 1,
                   Content = "Anasayfa",
                   IsDeleted = false,
                   AppLanguageId = 1,
                   Title = "Anasayfa",
                   Url = "anasayfa",
                   MetaTitle = "Anasayfa",
                   MetaDescription = "Anasayfa"
               },
               new AppPageTranslation {
                   Id = 2,
                   AppPageId = 1,
                   Content = "Home",
                   IsDeleted = false,
                   AppLanguageId = 2, 
                   Title  = "Home", 
                   Url = "home",
                   MetaTitle = "Home",
                   MetaDescription = "Home" }
            );
        }
    }

}
