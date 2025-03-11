using Economy.Domain.Entites.EntityAppMenus;
using Economy.Domain.Entites.EntityAppPages;
using Economy.Domain.Entites.EntityMenuItems;
using Economy.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Economy.Persistence.Seeds
{
    public class AppPageSeeder
    {
        private readonly AppDbContext _context;

        public AppPageSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            var appPages = await _context.AppPages.Include(x => x.Translations).ToListAsync();
            _context.AppPages.RemoveRange(appPages);
            await _context.SaveChangesAsync();

            if (!_context.AppPages.Any())
            {
                var app = GetPreconfiguredAppPages();
                await _context.AppPages.AddRangeAsync(app);
                await _context.SaveChangesAsync();
            }
        }

        private IEnumerable<AppPage> GetPreconfiguredAppPages()
        {
            return new List<AppPage>
            {
              new AppPage
            {
                IsHomePage = true,
                IsDeleted = false,
                Translations = new List<AppPageTranslation>
                {
                    new AppPageTranslation { AppLanguageId = 1, Title = "Anasayfa", Url = "anasayfa",MetaTitle="Anasayfa",MetaDescription="Anasayfa" },
                    new AppPageTranslation { AppLanguageId = 2, Title = "Home", Url = "home",MetaTitle="Home",MetaDescription="Home" }
                }
            },
            new AppPage
            {
                IsHomePage = false,
                IsDeleted = false,
                Translations = new List < AppPageTranslation > { new AppPageTranslation {AppLanguageId = 1, Title = "Anasayfa", Url = "anasayfa", MetaTitle = "Anasayfa", MetaDescription = "Anasayfa"},
                    new AppPageTranslation {AppLanguageId = 2, Title = "Home", Url = "home", MetaTitle = "Home", MetaDescription = "Home"} }
            }
            };
        }
    }

}
