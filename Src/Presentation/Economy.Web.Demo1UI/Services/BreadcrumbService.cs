namespace MyHotelSite.Services;

public record BreadcrumbItem(string Name, string Url);

public interface IBreadcrumbService
{
    string BuildJsonLd(params BreadcrumbItem[] items);
    string CombineJsonLd(params string?[] jsonLdParts);
}

public class BreadcrumbService : IBreadcrumbService
{
    public string BuildJsonLd(params BreadcrumbItem[] items)
    {
        var listItems = items.Select((it, idx) => new {
            @type = "ListItem",
            position = idx + 1,
            name = it.Name,
            item = it.Url
        });
        var obj = new
        {
            @context = "https://schema.org",
            @type = "BreadcrumbList",
            itemListElement = listItems
        };
        return System.Text.Json.JsonSerializer.Serialize(obj);
    }

    public string CombineJsonLd(params string?[] jsonLdParts)
    {
        var parts = jsonLdParts.Where(p => !string.IsNullOrWhiteSpace(p)).ToList();
        if (parts.Count == 0) return "";
        if (parts.Count == 1) return parts[0]!;
        var arr = "[" + string.Join(",", parts) + "]";
        return $$"""{"@context":"https://schema.org","@graph":{{arr}}}""";
    }
}
