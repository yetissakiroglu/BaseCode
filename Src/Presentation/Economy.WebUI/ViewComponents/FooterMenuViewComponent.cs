namespace Economy.Web.UI.ViewComponents
{
    using Economy.Web.UI.Models;
    using Economy.Web.UI.Services.Abstractions;
    using Microsoft.AspNetCore.Mvc;

    public class FooterMenuViewComponent : ViewComponent
    {
        private readonly IMenuService _menu;
        private readonly IPageService _pages;

        public FooterMenuViewComponent(IMenuService menu, IPageService pages)
        {
            _menu = menu; _pages = pages;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _menu.GetFooterAsync();
            var cultureFull = Thread.CurrentThread.CurrentUICulture.Name;
            var cultureShort = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
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
                list.Add(new MenuLinkVm(title, href, it.External, Active: false, Children: null));
            }
            return list;
        }

        private async Task<string> ResolveHrefAsync(MenuItem it, string cultureShort)
        {
            if (it.PageID.HasValue)
            {
                var slug = await _pages.GetSlugAsync(it.PageID.Value, cultureShort);
                if (!string.IsNullOrEmpty(slug)) return slug!;
            }
            if (!string.IsNullOrWhiteSpace(it.Url)) return it.Url!;
            if (!string.IsNullOrEmpty(it.Controller) && !string.IsNullOrEmpty(it.Action))
            {
                var rv = new RouteValueDictionary(it.RouteValues ?? new { });
                rv["culture"] = cultureShort;
                return Url.Action(it.Action, it.Controller, rv) ?? "#";
            }
            return "#";
        }
    }

}
