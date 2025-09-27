using Economy.UI.Models;
using Economy.Web.Demo1UI.Helpers;
using MyHotelSite.Repositories;

namespace MyHotelSite.Services;

public record MenuNode(string Title, string Url, bool IsExternal, bool IsActive, List<MenuNode> Children)
{
    public bool Selected { get; set; }        // tam eşleşen sayfa
    public bool BranchSelected { get; set; }  // kendisi veya çocuklarından biri aktif
}

public interface IMenuService
{
    Task<List<MenuNode>> GetTreeAsync(int appId, string lang);
}

public class MenuService : IMenuService
{
    private readonly IApiClientHelper _apiClient;
    private readonly IPageRepository _pages;
    private readonly IHttpContextAccessor _http;

    public MenuService(IApiClientHelper repo, IPageRepository pages, IHttpContextAccessor http)
    { _apiClient = repo; _pages = pages; _http = http; }

    private static string NormalizePath(string? p)
    {
        if (string.IsNullOrWhiteSpace(p)) return "/";
        var q = p.Split('#')[0].Split('?')[0];
        if (q.Length > 1 && q.EndsWith("/")) q = q[..^1];
        return q.ToLowerInvariant();
    }

    public async Task<List<MenuNode>> GetTreeAsync(int appId, string lang)
    {
        var items = await _apiClient.GetAsync<List<MenuItem>>("/api/menuconfig", lang);

        var byParent = items
            .OrderBy(i => i.Order)
            .ToLookup(i => i.ParentId); // null parent destekler

        var reqPath = NormalizePath(_http.HttpContext?.Request?.Path.Value ?? "/");

        async Task<MenuNode> Map(MenuItem x, int level)
        {
            string url = "#"; bool ext = x.IsExternal;

            if (ext && !string.IsNullOrWhiteSpace(x.Url))
            {
                url = x.Url!;
            }
            else if (x.PageId.HasValue)
            {
                var p = await _pages.GetAsync(appId, lang, x.PageId.Value);
                if (p != null) url = $"/{p.Lang}/{p.SectionKey}/{p.Slug}";
            }else
            {
                url = x.Url!;
            }

            var node = new MenuNode(x.Title ?? "", url, ext, x.IsActive, new());

            if (level < 3)
            {
                foreach (var c in byParent[x.Id])
                    node.Children.Add(await Map(c, level + 1));
            }

            // Aktiflik: sadece internal URL’lerde değerlendir
            if (node.IsExternal && !string.IsNullOrWhiteSpace(node.Url) && node.Url.StartsWith("/"))
            {
                var my = NormalizePath(node.Url);
                var exact = reqPath == my;
                var under = !exact && reqPath.StartsWith(my + "/", StringComparison.Ordinal);

                node.Selected = exact;
                node.BranchSelected = exact || under || node.Children.Any(ch => ch.Selected || ch.BranchSelected);
            }
            else
            {
                node.Selected = false;
                node.BranchSelected = node.Children.Any(ch => ch.Selected || ch.BranchSelected);
            }

            return node;
        }

        var list = new List<MenuNode>();
        foreach (var root in byParent[null]) list.Add(await Map(root, 1));
        return list;
    }
}