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
            var menusDelete = await _context.AppMenus.Where(w=>w.ParentMenuId==null).ToListAsync();
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
            Title = "Ana Menü",
            Slug = "ana-menu",
            IsExternal = false,
            ParentMenuId = null
        },
        new AppMenu
        {
            Title = "Odalar & Süitler",
            Slug = "odalar-suitler",
            IsExternal = false,
            ParentMenuId = null
        },
        new AppMenu
        {
            Title = "Restoran & Bar",
            Slug = "restoran-bar",
            IsExternal = false,
            ParentMenuId = null
        },
        new AppMenu
        {
            Title = "Spa & Wellness",
            Slug = "spa-wellness",
            IsExternal = false,
            ParentMenuId = null
        },
  
        new AppMenu
        {
            Title = "Hakkımızda",
            Slug = "hakkimizda",
            IsExternal = false,
            ParentMenuId = null
        },
        new AppMenu
        {
            Title = "İletişim",
            Slug = "iletisim",
            IsExternal = false,
            ParentMenuId = null
        }
    };
        }
    }

}
