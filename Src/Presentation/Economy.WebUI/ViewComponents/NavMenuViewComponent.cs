using Economy.Web.UI.Models;
using Economy.Web.UI.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
namespace Economy.Web.UI.ViewComponents
{
   

    public class NavMenuViewComponent : ViewComponent
    {
        private readonly IMenuService _menu;
        private readonly IPageService _pages;

        public NavMenuViewComponent(IMenuService menu, IPageService pages)
        {
            _menu = menu; _pages = pages;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _menu.GetMainAsync();
            var cultureFull = Thread.CurrentThread.CurrentUICulture.Name;             // "tr-TR"
            var cultureShort = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName; // "tr"

            var vms = await ResolveAsync(items, cultureFull, cultureShort);
            return View(vms);
        }

        private async Task<List<MenuLinkVm>> ResolveAsync(List<MenuItem> items, string cultureFull, string cultureShort)
        {
            var list = new List<MenuLinkVm>();
            foreach (var it in items)
            {
                var title = _menu.TitleFor(it, cultureFull);
                var href = await ResolveHrefAsync(it, cultureShort);
                var active = IsActive(href);

                List<MenuLinkVm>? children = null;
                if (it.Children != null && it.Children.Any())
                    children = await ResolveAsync(it.Children, cultureFull, cultureShort);

                list.Add(new MenuLinkVm(title, href, it.External, active, children));
            }
            return list;
        }

        private async Task<string> ResolveHrefAsync(MenuItem it, string cultureShort)
        {
            // 1) PageID öncelikli
            if (it.PageID.HasValue)
            {
                var slug = await _pages.GetSlugAsync(it.PageID.Value, cultureShort);
                if (!string.IsNullOrEmpty(slug)) return slug!;
            }
            // 2) Harici URL
            if (!string.IsNullOrWhiteSpace(it.Url)) return it.Url!;

            // 3) Controller/Action
            if (!string.IsNullOrEmpty(it.Controller) && !string.IsNullOrEmpty(it.Action))
            {
                var rv = new RouteValueDictionary(it.RouteValues ?? new { });
                rv["culture"] = cultureShort;
                return Url.Action(it.Action, it.Controller, rv) ?? "#";
            }
            return "#";
        }

        private bool IsActive(string href)
        {
            var currentPath = (HttpContext.Request.Path.Value ?? "").TrimEnd('/');
            var linkPath = href?.Split('?')[0]?.TrimEnd('/') ?? "#";
            if (string.IsNullOrEmpty(linkPath) || linkPath == "#") return false;

            // Basit eşleştirme: tam eşitlik ya da current linkPath ile başlıyor
            return string.Equals(currentPath, linkPath, StringComparison.OrdinalIgnoreCase) ||
                   (currentPath.StartsWith(linkPath, StringComparison.OrdinalIgnoreCase) && linkPath.Length > 1);
        }
    }

}
