using Economy.Domain.Entites.EntitySlides;
using Economy.Persistence.Contexts;

namespace Economy.Persistence.Seeds
{
    public class AppSlideSeeder
    {
        private readonly DefaultDbContext _context;

        public AppSlideSeeder(DefaultDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            //if (!_context.AppSlides.Any())
            //{
            //    var slides = GetPreconfiguredSlides();
            //    await _context.AppSlides.AddRangeAsync(slides);
            //    await _context.SaveChangesAsync();
            //}
        }

        private IEnumerable<AppSlide> GetPreconfiguredSlides()
        {
            return new List<AppSlide>
    {

        new AppSlide
        {
            Sequence = 1,
            Translations = new List<AppSlideTranslation>
            {
                new AppSlideTranslation {
                Title = "Hoş Geldiniz",
                Content = "En iyi tatil deneyimi için bizimle olun.",
                IsExternal = false,
                ButtonText = "Keşfet",
                ButtonUrl = "/explore",
                ButtonIcon = "fa-search",
                AppLanguageId = 1,
                },
               new AppSlideTranslation {
                Title = "Welcome",
                Content = "Join us for the best vacation experience.",
                IsExternal = false,
                ButtonText = "Explore",
                ButtonUrl = "/explore",
                ButtonIcon = "fa-search",
                AppLanguageId = 2,
                }
            }
        },
                new AppSlide
        {
            Sequence = 1,
            Translations = new List<AppSlideTranslation>
            {
                new AppSlideTranslation {
                Title = "Hoş Geldiniz",
                Content = "En iyi tatil deneyimi için bizimle olun.",
                IsExternal = false,
                ButtonText = "Keşfet",
                ButtonUrl = "/explore",
                ButtonIcon = "fa-search",
                AppLanguageId = 1,
                },
               new AppSlideTranslation {
                Title = "Welcome",
                Content = "Join us for the best vacation experience.",
                IsExternal = false,
                ButtonText = "Explore",
                ButtonUrl = "/explore",
                ButtonIcon = "fa-search",
                AppLanguageId = 2,
                }
            }
        },
                                new AppSlide
        {
            Sequence = 1,
            Translations = new List<AppSlideTranslation>
            {
                new AppSlideTranslation {
                Title = "Hoş Geldiniz",
                Content = "En iyi tatil deneyimi için bizimle olun.",
                IsExternal = false,
                ButtonText = "Keşfet",
                ButtonUrl = "/explore",
                ButtonIcon = "fa-search",
                AppLanguageId = 1,
                },
               new AppSlideTranslation {
                Title = "Welcome",
                Content = "Join us for the best vacation experience.",
                IsExternal = false,
                ButtonText = "Explore",
                ButtonUrl = "/explore",
                ButtonIcon = "fa-search",
                AppLanguageId = 2,
                }
            }
        }



    };
        }
    }

  
}
