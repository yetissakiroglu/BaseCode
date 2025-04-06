using Economy.Domain.Entites.EntityAppSettings;
using Economy.Domain.Entites.EntitySlides;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Configurations.ConfigurationAppSlide
{
    public class AppAppSlideTranslation_Configuration : IEntityTypeConfiguration<AppSlideTranslation>
    {
        public void Configure(EntityTypeBuilder<AppSlideTranslation> builder)
        {
            builder.ToTable("AppSlideTranslations");

            builder.HasKey(x => x.Id);


            var models = new List<AppSlideTranslation>
            {
                new AppSlideTranslation {AppSlideId = 1,
                Id = 1,
                IsDeleted = false,
                Title = "Hoş Geldiniz",
                Content = "En iyi tatil deneyimi için bizimle olun.",
                IsExternal = false,
                ButtonText = "Keşfet",
                ButtonUrl = "/explore",
                ButtonIcon = "fa-search",
                AppLanguageId = 1,
                },
               new AppSlideTranslation {AppSlideId = 1,Id = 2,IsDeleted = false,
                Title = "Welcome",
                Content = "Join us for the best vacation experience.",
                IsExternal = false,
                ButtonText = "Explore",
                ButtonUrl = "/explore",
                ButtonIcon = "fa-search",
                AppLanguageId = 2,
                },
                new AppSlideTranslation {AppSlideId = 2,Id = 3,IsDeleted = false,

                Title = "Hoş Geldiniz",
                Content = "En iyi tatil deneyimi için bizimle olun.",
                IsExternal = false,
                ButtonText = "Keşfet",
                ButtonUrl = "/explore",
                ButtonIcon = "fa-search",
                AppLanguageId = 1,
                },
               new AppSlideTranslation {AppSlideId = 2,Id = 4,IsDeleted = false,
                Title = "Welcome",
                Content = "Join us for the best vacation experience.",
                IsExternal = false,
                ButtonText = "Explore",
                ButtonUrl = "/explore",
                ButtonIcon = "fa-search",
                AppLanguageId = 2,
                },
                  new AppSlideTranslation {AppSlideId = 3,Id = 5,IsDeleted = false,
                Title = "Hoş Geldiniz",
                Content = "En iyi tatil deneyimi için bizimle olun.",
                IsExternal = false,
                ButtonText = "Keşfet",
                ButtonUrl = "/explore",
                ButtonIcon = "fa-search",
                AppLanguageId = 1,
                },
               new AppSlideTranslation {AppSlideId = 3,Id = 6,IsDeleted = false,
                Title = "Welcome",
                Content = "Join us for the best vacation experience.",
                IsExternal = false,
                ButtonText = "Explore",
                ButtonUrl = "/explore",
                ButtonIcon = "fa-search",
                AppLanguageId = 2,
                }
            };


            // Seed Data
            builder.HasData(models);
        }
    }


}
