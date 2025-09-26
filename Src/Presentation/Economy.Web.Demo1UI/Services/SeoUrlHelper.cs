using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace MyHotelSite.Services;

public interface ISeoUrlHelper
{
    string Slugify(string text, string lang, bool preserveUnicode = false);
    string BuildSeoPath(string lang, string sectionKey, string title, int? idTail = null, bool preserveUnicode = false);
}

public class SeoUrlHelper : ISeoUrlHelper
{
    private static readonly Dictionary<string, Dictionary<string, string>> SectionMap = new()
    {
        ["room"] = new() { ["tr"] = "oda", ["en"] = "room", ["de"] = "zimmer" },
        ["campaign"] = new() { ["tr"] = "kampanya", ["en"] = "campaign", ["de"] = "aktion" },
        ["page"] = new() { ["tr"] = "sayfa", ["en"] = "page", ["de"] = "seite" }
    };

    public string Slugify(string text, string lang, bool preserveUnicode = false)
    {
        if (string.IsNullOrWhiteSpace(text)) return "";
        var s = Regex.Replace(text.Trim(), @"\s+", " ");
        if (preserveUnicode)
        {
            s = s.ToLower(new CultureInfo(lang == "tr" ? "tr-TR" : "en-US")).Replace(' ', '-');
            var sb = new StringBuilder();
            foreach (var ch in s.Normalize(NormalizationForm.FormC))
                if (char.IsLetterOrDigit(ch) || ch == '-') sb.Append(ch);
            return Regex.Replace(sb.ToString(), "-{2,}", "-").Trim('-');
        }
        var ascii = s.Normalize(NormalizationForm.FormD);
        var sb2 = new StringBuilder();
        foreach (var c in ascii)
        {
            var cat = CharUnicodeInfo.GetUnicodeCategory(c);
            if (cat != UnicodeCategory.NonSpacingMark) sb2.Append(c);
        }
        var res = Regex.Replace(sb2.ToString(), @"[^\w\s-]", "").ToLowerInvariant().Replace(' ', '-');
        return Regex.Replace(res, "-{2,}", "-").Trim('-');
    }

    public string BuildSeoPath(string lang, string sectionKey, string title, int? idTail = null, bool preserveUnicode = false)
    {
        var section = SectionMap.TryGetValue(sectionKey, out var m) && m.TryGetValue(lang, out var localized) ? localized : sectionKey;
        var slug = Slugify(title, lang, preserveUnicode);
        if (idTail is int id && id > 0) slug = $"{slug}-{id}";
        return $"/{lang}/{section}/{slug}";
    }
}
