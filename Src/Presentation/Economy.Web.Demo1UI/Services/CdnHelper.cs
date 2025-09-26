using MyHotelSite.Models;

namespace MyHotelSite.Services;

public interface ICdnHelper
{
    string Url(string path, bool withVersion = true);
    string Img(string path, int? w = null, string? format = null);
    string SrcSet(string path, int[] widths, string? format = null);
    string PictureHtml(string path, int[] widths, string sizes, string alt, bool eagerLcp = false);
}

public class CdnHelper : ICdnHelper
{
    private readonly ISiteConfigAccessor _cfg;
    public CdnHelper(ISiteConfigAccessor cfg) { _cfg = cfg; }

    public string Url(string path, bool withVersion = true)
    {
        var cfg = _cfg.GetAsync(1).GetAwaiter().GetResult();
        var baseUrl = cfg.Technical?.CdnBaseUrl;
        var version = cfg.Technical?.CdnEnabled == true ? $"v={DateTime.UtcNow:yyyyMMdd}" : null;

        if (string.IsNullOrWhiteSpace(path)) return "";
        if (path.StartsWith("http://") || path.StartsWith("https://") || path.StartsWith("//")) return path;
        if (string.IsNullOrWhiteSpace(baseUrl))
            return withVersion && version != null ? $"{path}?{version}" : path;

        var u = baseUrl.TrimEnd('/') + (path.StartsWith("/") ? path : "/" + path);
        return withVersion && version != null ? $"{u}?{version}" : u;
    }

    public string Img(string path, int? w = null, string? format = null)
    {
        var url = Url(path, withVersion: false);
        var qs = new List<string>();
        if (w.HasValue) qs.Add($"w={w.Value}");
        if (!string.IsNullOrWhiteSpace(format)) qs.Add($"format={format}");
        var q = qs.Count > 0 ? "?" + string.Join("&", qs) : "";
        return url + q;
    }

    public string SrcSet(string path, int[] widths, string? format = null)
        => string.Join(", ", widths.Distinct().OrderBy(x => x).Select(w => $"{Img(path, w, format)} {w}w"));

    public string PictureHtml(string path, int[] widths, string sizes, string alt, bool eagerLcp = false)
    {
        var avif = SrcSet(path, widths, "avif");
        var webp = SrcSet(path, widths, "webp");
        var std = SrcSet(path, widths, null);
        var loading = eagerLcp ? @"fetchpriority=""high""" : @"loading=""lazy""";
        var lcpAttr = eagerLcp ? @"decoding=""sync""" : @"decoding=""async""";

        return $@"
<picture>
  <source type=""image/avif"" srcset=""{avif}"" sizes=""{System.Net.WebUtility.HtmlEncode(sizes)}"">
  <source type=""image/webp"" srcset=""{webp}"" sizes=""{System.Net.WebUtility.HtmlEncode(sizes)}"">
  <img src=""{Img(path, widths.Min())}"" srcset=""{std}"" sizes=""{System.Net.WebUtility.HtmlEncode(sizes)}""
       alt=""{System.Net.WebUtility.HtmlEncode(alt)}"" {loading} {lcpAttr} style=""width:100%;height:auto"">
</picture>";
    }
}
