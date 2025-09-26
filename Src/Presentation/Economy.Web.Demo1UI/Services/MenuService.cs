using MyHotelSite.Models;
using MyHotelSite.Repositories;

namespace MyHotelSite.Services;

public record MenuNode(string Title, string Url, bool IsExternal, List<MenuNode> Children);

public interface IMenuService
{
    Task<List<MenuNode>> GetTreeAsync(int appId, string lang);
}

public class MenuService : IMenuService
{
    private readonly IMenuRepository _repo;
    private readonly IPageRepository _pages;
    public MenuService(IMenuRepository repo, IPageRepository pages) { _repo = repo; _pages = pages; }

    public async Task<List<MenuNode>> GetTreeAsync(int appId, string lang)
    {
        var items = await _repo.GetAsync(appId, lang);
        //var byParent = items.GroupBy(i => i.ParentId).ToDictionary(g => g.Key, g => g.OrderBy(x => x.Order).ToList());
        var byParent = items
       .OrderBy(x => x.Order)
       .ToLookup(i => i.ParentId); // ILookup<int?, MenuItem>
        async Task<MenuNode> Map(MenuItem x, int level)
        {
            string url = "#"; bool ext = x.IsExternal;
            if (x.IsExternal && !string.IsNullOrWhiteSpace(x.ExternalUrl)) url = x.ExternalUrl!;
            else if (x.PageId.HasValue)
            {
                var p = await _pages.GetAsync(appId, lang, x.PageId.Value);
                if (p != null) url = $"/{p.Lang}/{p.SectionKey}/{p.Slug}";
            }

            var node = new MenuNode(x.Title, url, ext, new());
            if (level < 3)
            {
                // çocuklar: null key sorunu yok, doğrudan lookup kullan
                foreach (var c in byParent[x.Id])
                    node.Children.Add(await Map(c, level + 1));
            }
            return node;
        }

        // kökler: ParentId == null
        var roots = byParent[null];
        var list = new List<MenuNode>();
        foreach (var root in roots) list.Add(await Map(root, 1));
        return list;

        //async Task<MenuNode> Map(MenuItem x, int level)
        //{
        //    string url = "#"; bool ext = x.IsExternal;
        //    if (x.IsExternal && !string.IsNullOrWhiteSpace(x.ExternalUrl)) url = x.ExternalUrl!;
        //    else if (x.PageId.HasValue)
        //    {
        //        var p = await _pages.GetAsync(appId, lang, x.PageId.Value);
        //        if (p != null) url = $"/{p.Lang}/{p.SectionKey}/{p.Slug}";
        //    }
        //    var node = new MenuNode(x.Title, url, ext, new());
        //    if (level < 3 && byParent.TryGetValue(x.Id, out var children))
        //        foreach (var c in children) node.Children.Add(await Map(c, level + 1));
        //    return node;
        //}

        //var roots = byParent.TryGetValue(null, out var r) ? r : new List<MenuItem>();
        //var list = new List<MenuNode>();
        //foreach (var root in roots) list.Add(await Map(root, 1));
        //return list;
    }
}
