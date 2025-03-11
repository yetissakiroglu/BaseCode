using Economy.Domain.Entites.EntityAppMenus;
using Economy.Domain.Entites.EntityMenuItems;
using Economy.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Economy.Persistence.Seeds
{
    public class AppMenuSeeder
    {
        private readonly AppDbContext _context;

        public AppMenuSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            var menusDelete = await _context.AppMenus.Include(x=>x.SubMenus).Include(x => x.ParentMenu).Include(x => x.Translations).Where(w=>w.ParentMenuId==null).ToListAsync();
            _context.AppMenus.RemoveRange(menusDelete);
            await _context.SaveChangesAsync();

            if (!_context.AppMenus.Any())
            {
                var menus = GetPreconfiguredMenus();


                await _context.AppMenus.AddRangeAsync(menus);
                await _context.SaveChangesAsync();
            }
        }

        private IEnumerable<AppMenu> GetPreconfiguredMenus()
        {
            return new List<AppMenu>
    {
        new AppMenu
        {
            IsExternal = false,
            ParentMenuId = null,
            Translations = new List<AppMenuTranslation>
            {
                new AppMenuTranslation { AppLanguageId = 1, Title = "Ana Menü", Url = "ana-menu" },
                new AppMenuTranslation { AppLanguageId = 2, Title = "Main Menu", Url = "main-menu" }
            }
        },
        new AppMenu
        {
            IsExternal = false,
            ParentMenuId = null,
            Translations = new List<AppMenuTranslation>
            {
                new AppMenuTranslation { AppLanguageId = 1, Title = "Odalar & Süitler", Url = "odalar-suitler" },
                new AppMenuTranslation { AppLanguageId = 2, Title = "Rooms & Suites", Url = "rooms-suites" }
            }
        },
        new AppMenu
        {
            IsExternal = false,
            ParentMenuId = null,
            Translations = new List<AppMenuTranslation>
            {
                new AppMenuTranslation { AppLanguageId = 1, Title = "Restoran & Bar", Url = "restoran-bar" },
                new AppMenuTranslation { AppLanguageId = 2, Title = "Restaurant & Bar", Url = "restaurant-bar" }
            }
        },
        new AppMenu
        {
            IsExternal = false,
            ParentMenuId = null,
            Translations = new List<AppMenuTranslation>
            {
                new AppMenuTranslation { AppLanguageId = 1, Title = "Spa & Wellness", Url = "spa-wellness" },
                new AppMenuTranslation { AppLanguageId = 2, Title = "Spa & Wellness", Url = "spa-wellness" }
            }
        },
        new AppMenu
        {
            IsExternal = false,
            ParentMenuId = null,
            Translations = new List<AppMenuTranslation>
            {
                new AppMenuTranslation { AppLanguageId = 1, Title = "Hakkımızda", Url = "hakkimizda" },
                new AppMenuTranslation { AppLanguageId = 2, Title = "About Us", Url = "about-us" }
            }
        },
        new AppMenu
        {
            IsExternal = false,
            ParentMenuId = null,
            Translations = new List<AppMenuTranslation>
            {
                new AppMenuTranslation { AppLanguageId = 1, Title = "İletişim", Url = "iletisim" },
                new AppMenuTranslation { AppLanguageId = 2, Title = "Contact", Url = "contact" }
            }
        }
    };
        }
    }

}
