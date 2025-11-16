using Economy.UI.Dtos;

namespace HotelMultiTenant.Multitenancy
{
    public sealed class SeoHelper : ISeoHelper
    {
        public Task<SeoDto> BuildAsync(
            IReadOnlyList<HreflangVm> hreflang,
            SeoSeed seed,
            string currentLang,
            string? xDefaultUrl = null)
        {
            if (hreflang is null || hreflang.Count == 0)
                throw new ArgumentException("En az bir hreflang kaydı gönderilmelidir.", nameof(hreflang));

            if (string.IsNullOrWhiteSpace(currentLang))
                throw new ArgumentNullException(nameof(currentLang));

            var currentLangLower = currentLang.ToLowerInvariant();

            // 1) Canonical: currentLang'e ait URL, yoksa ilk geçerli URL
            var canonicalVm = hreflang
                .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.Lang)
                                     && x.Lang.Equals(currentLang, StringComparison.OrdinalIgnoreCase)
                                     && !string.IsNullOrWhiteSpace(x.Url))
                ?? hreflang.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.Url));

            if (canonicalVm is null)
                throw new InvalidOperationException("Canonical için kullanılabilir bir URL bulunamadı.");

            var canonical = canonicalVm.Url;

            // 2) Hreflang listesi (dile göre tekil, URL dolu olanlar)
            var hreflangList = hreflang
                .Where(x => !string.IsNullOrWhiteSpace(x.Lang) &&
                            !string.IsNullOrWhiteSpace(x.Url))
                .GroupBy(x => x.Lang.Trim().ToLowerInvariant())
                .Select(g =>
                {
                    var lang = g.Key;                // "tr", "en"...
                    var url = g.First().Url;        // ilk URL'i al
                    return (lang, url);
                })
                .ToList();

            // x-default varsa ekle
            if (!string.IsNullOrWhiteSpace(xDefaultUrl))
                hreflangList.Add(("x-default", xDefaultUrl!));

            // 3) DTO oluştur
            var dto = new SeoDto
            {
                MetaTitle = seed.Title,
                MetaDescription = seed.Description,
                CanonicalUrl = canonical,
                ShareImage = seed.ShareImage,
                OgType = seed.OgType,
                Hreflangs = hreflangList,
                JsonLd = seed.JsonLd
            };

            return Task.FromResult(dto);
        }
    }
}
