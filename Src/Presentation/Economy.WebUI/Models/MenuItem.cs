namespace Economy.Web.UI.Models
{
    public record MenuItem(
    string Key,
    Dictionary<string, string> Titles,   // "tr-TR" / "en-US"
    string? Controller = null,
    string? Action = null,
    object? RouteValues = null,
    string? Url = null,
    bool External = false,
    int? PageID = null,
    List<MenuItem>? Children = null
);

    // VC -> View için çözümlenmiş link modeli
    public record MenuLinkVm(
        string Title,
        string Href,
        bool External,
        bool Active,
        List<MenuLinkVm>? Children = null
    );
}
