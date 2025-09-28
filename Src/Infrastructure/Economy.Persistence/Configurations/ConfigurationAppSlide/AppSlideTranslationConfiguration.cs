using Economy.Domain.Entites.EntitySlides;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Configurations.ConfigurationAppSlide
{
    public class AppSlideTranslationConfiguration : IEntityTypeConfiguration<AppSlideTranslation>
    {
        public void Configure(EntityTypeBuilder<AppSlideTranslation> builder)
        {
            builder.ToTable("AppSlideTranslations");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title).IsRequired().HasMaxLength(250);
            builder.Property(x => x.ButtonText).HasMaxLength(100);


            builder.HasData(
                // Slide 1
                new AppSlideTranslation
                {
                    Id = 1,
                    AppSlideId = 1,
                    AppLanguageId = 1, // EN
                    Title = "Welcome",
                
                },
                new AppSlideTranslation
                {
                    Id = 2,
                    AppSlideId = 1,
                    AppLanguageId = 2, // TR
                    Title = "Hoş Geldiniz",
                
                    ButtonText = "Keşfet",
   
                },

                // Slide 2
                new AppSlideTranslation
                {
                    Id = 3,
                    AppSlideId = 2,
                    AppLanguageId = 1,
                    Title = "Our Services",
              
                    ButtonText = "View Services",
              
                },
                new AppSlideTranslation
                {
                    Id = 4,
                    AppSlideId = 2,
                    AppLanguageId = 2,
                    Title = "Hizmetlerimiz",
                    //Content = "Sunduğumuz hizmetleri keşfedin.",
                    //IsExternal = false,
                    //ButtonText = "Hizmetleri Gör",
                    //ButtonUrl = "/hizmetler",
                    //ButtonIcon = "service"
                },

                // Slide 3
                new AppSlideTranslation
                {
                    Id = 5,
                    AppSlideId = 3,
                    AppLanguageId = 1,
                    Title = "Contact Us",
                    //Content = "We are here to help.",
                    //IsExternal = true,
                    //ButtonText = "Get in Touch",
                    //ButtonUrl = "https://contact.example.com",
                    //ButtonIcon = "mail"
                },
                new AppSlideTranslation
                {
                    Id = 6,
                    AppSlideId = 3,
                    AppLanguageId = 2,
                    Title = "Bize Ulaşın",
                    //Content = "Size yardımcı olmak için buradayız.",
                    //IsExternal = true,
                    ButtonText = "İletişime Geç",
                    //ButtonUrl = "https://iletisim.example.com",
                    //ButtonIcon = "mail"
                }
            );
        }
    }


}
