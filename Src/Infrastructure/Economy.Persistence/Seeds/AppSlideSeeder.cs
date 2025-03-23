using Economy.Domain.Entites.EntityAppMenus;
using Economy.Domain.Entites.EntityMenuItems;
using Economy.Domain.Entites.EntitySlides;
using Economy.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Economy.Persistence.Seeds
{
    public class AppSlideSeeder
    {
        private readonly AppDbContext _context;

        public AppSlideSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            var slideDelete = await _context.AppSlides.Include(x => x.Translations).ToListAsync();
            _context.AppSlides.RemoveRange(slideDelete);
            await _context.SaveChangesAsync();

            if (!_context.AppSlides.Any())
            {
                var slides = GetPreconfiguredSlides();
                await _context.AppSlides.AddRangeAsync(slides);
                await _context.SaveChangesAsync();
            }
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
                Thumbnail = "slide_1.jpg",
                IsExternal = false,
                ButtonText = "Keşfet",
                ButtonUrl = "/explore",
                ButtonIcon = "fa-search",
                AppLanguageId = 1,
                },
               new AppSlideTranslation {
                Title = "Welcome",
                Content = "Join us for the best vacation experience.",
                Thumbnail = "slide_1.jpg",
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
                Thumbnail = "slide_2.jpg",
                IsExternal = false,
                ButtonText = "Keşfet",
                ButtonUrl = "/explore",
                ButtonIcon = "fa-search",
                AppLanguageId = 1,
                },
               new AppSlideTranslation {
                Title = "Welcome",
                Content = "Join us for the best vacation experience.",
                Thumbnail = "slide_2.jpg",
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
                Thumbnail = "slide_3.jpg",
                IsExternal = false,
                ButtonText = "Keşfet",
                ButtonUrl = "/explore",
                ButtonIcon = "fa-search",
                AppLanguageId = 1,
                },
               new AppSlideTranslation {
                Title = "Welcome",
                Content = "Join us for the best vacation experience.",
                Thumbnail = "slide_3.jpg",
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
