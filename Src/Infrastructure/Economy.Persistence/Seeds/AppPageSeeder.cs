using Economy.Domain.Entites.EntityAppPages;
using Economy.Persistence.Contexts;

namespace Economy.Persistence.Seeds
{
    public class AppPageSeeder
    {
        private readonly DefaultDbContext _context;

        public AppPageSeeder(DefaultDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            //if (!_context.AppPages.Any())
            //{
            //    var app = GetPreconfiguredAppPages();
            //    await _context.AppPages.AddRangeAsync(app);
            //    await _context.SaveChangesAsync();
            //}
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
                Translations = new List<AppPageTranslation> {
                          new AppPageTranslation {AppLanguageId = 1, Title = "Slide tr", Url = "Slide tr", MetaTitle = "Slide tr", MetaDescription = "Slide tr"},
                          new AppPageTranslation {AppLanguageId = 2, Title = "Slide en", Url = "Slide en", MetaTitle = "Slide en", MetaDescription = "Slide en"} }
            }
            };
        }
    }

}
