using Economy.Domain.Entites.EntityMenuItems;
using Economy.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Title = "Hakkımızda",
                Slug = "hakkimizda",
                IsExternal = false,
                ParentMenuId = 1
            },
            new AppMenu
            {
                Title = "İletişim",
                Slug = "iletisim",
                IsExternal = true,
                ParentMenuId = 1
            }
        };
        }
    }

}
