using Economy.Application.ApplicationUI.Interfaces;
using Economy.Core.Enums;
using Economy.Core.Interfaces;
using Economy.Domain.Entites.EntityAppLanguage;
using Economy.Domain.Entites.EntityAppNewPages;
using Economy.UI.Models.PageDtos;
using Microsoft.EntityFrameworkCore;

namespace Economy.Persistence.PersistenceUI.Services
{


    public sealed class PageAccessor : IPageAccessor
    {
        private readonly IEntityRepository<AppLanguage, int> _langRepo;
        private readonly IEntityRepository<ContentItem, int> _contentRepo;
        private readonly IEntityRepository<ContentItemTranslation, int> _trRepo;
        private readonly IEntityRepository<ContentMedia, int> _mediaRepo;
        private readonly IEntityRepository<ContentMediaTranslation, int> _mediaTrRepo;

        public PageAccessor(IUnitOfWork uow)
        {
            _langRepo = uow.HotelEntityRepository<AppLanguage>();
            _contentRepo = uow.HotelEntityRepository<ContentItem>();
            _trRepo = uow.HotelEntityRepository<ContentItemTranslation>();
            _mediaRepo = uow.HotelEntityRepository<ContentMedia>();
            _mediaTrRepo = uow.HotelEntityRepository<ContentMediaTranslation>();
        }

        public async Task<PageUnifiedVm?> GetAsync(string lang, string slug, CancellationToken ct = default)
        {
            var want = (lang ?? "tr").ToLowerInvariant();
            var l = await _langRepo.DataSet
                .Where(x => !x.IsDeleted && x.IsActive && x.Code.ToLower() == want)
                .Select(x => new { x.Id, x.Code })
                .FirstOrDefaultAsync(ct)
                ?? await _langRepo.DataSet
                     .Where(x => !x.IsDeleted && x.IsActive && x.IsDefault)
                     .Select(x => new { x.Id, x.Code })
                     .FirstOrDefaultAsync(ct)
                ?? new { Id = 1, Code = "tr" };

            var langId = l.Id;
            var langCode = l.Code;

            // 1) Slug'a göre sayfayı bul (detay ya da liste başlığı olabilir)
            var hit = await (from ci in _contentRepo.DataSet
                             where !ci.IsDeleted && ci.IsActive && ci.Type == ContentItemType.Page
                             join tr in _trRepo.DataSet on ci.Id equals tr.ContentItemId
                             where !tr.IsDeleted && tr.IsActive && tr.LanguageId == langId && tr.Slug == slug
                             select new { ci, tr })
                            .FirstOrDefaultAsync(ct);

            if (hit is null) return null;

            // 2) Bu sayfanın çocukları var mı? (OwnerType=Content, OwnerId=this.Id)
            var childCount = await _contentRepo.DataSet
                .Where(x => !x.IsDeleted && x.IsActive
                            && x.Type == ContentItemType.Page
                            && x.OwnerType == ContentOwnerType.Content
                            && x.OwnerId == hit.ci.Id)
                .CountAsync(ct);

            if (childCount > 0)
            {
                // Bu bir LİSTE sayfasıdır → genel list builder ile dön
                return await BuildListByHeaderAsync(langId, langCode, hit.ci, hit.tr, ct);
            }

            // 3) Çocuğu yoksa DETAY sayfasıdır → mevcut detail akışın
            return await BuildDetailByHitAsync(langId, langCode, hit.ci, hit.tr, ct);
        }

        // ---------------- helpers ----------------
        private async Task<PageUnifiedVm> BuildListByHeaderAsync(
    int langId, string langCode,
    ContentItem headerCi, ContentItemTranslation headerTr,
    CancellationToken ct)
        {
            // 1) Bu başlığa bağlı çocuk sayfaları getir
            var items = await (from ci in _contentRepo.DataSet
                               where !ci.IsDeleted && ci.IsActive
                                     && ci.Type == ContentItemType.Page
                                     && ci.OwnerType == ContentOwnerType.Content
                                     && ci.OwnerId == headerCi.Id
                               join tr in _trRepo.DataSet on ci.Id equals tr.ContentItemId
                               where !tr.IsDeleted && tr.IsActive && tr.LanguageId == langId
                               orderby ci.SortOrder, ci.Id
                               select new PageListItemVm
                               {
                                   Id = ci.Id,
                                   Slug = tr.Slug ?? "",
                                   Title = tr.Title ?? "",
                                   Summary = tr.Summary,
                                   Image = tr.Image,
                                   PublishAtUtc = ci.PublishAtUtc,
                                   IsActive = ci.IsActive
                               })
                              .ToListAsync(ct);

            // 2) contentType'ı dinamik verelim: rooms/campaigns/restoranlar …
            // UI'in beklediği sabit değer yoksa slug'ı kullanmak en pratik olanı:
            var contentType = headerTr.Slug?.ToLowerInvariant() ?? "list";

            // 3) hreflangs
            var hreflangs = await (from t in _trRepo.DataSet
                                   where !t.IsDeleted && t.IsActive && t.ContentItemId == headerCi.Id
                                   join la in _langRepo.DataSet on t.LanguageId equals la.Id
                                   where !la.IsDeleted && la.IsActive
                                   orderby la.Code
                                   select new HreflangVm
                                   {
                                       Lang = la.Code,
                                       Slug = t.Slug ?? "",
                                       Url = ""
                                   }).ToListAsync(ct);

            // 4) VM
            return new PageUnifiedVm
            {
                Type = "list",
                ContentType = contentType,
                Items = items,

                Id = headerCi.Id,
                Lang = langCode,
                Slug = headerTr.Slug,
                Title = headerTr.Title,
                Summary = headerTr.Summary,
                Body = headerTr.Body,
                Image = headerTr.Image,

                MetaTitle = string.IsNullOrWhiteSpace(headerTr.MetaTitle) ? headerTr.Title : headerTr.MetaTitle,
                MetaDescription = headerTr.MetaDescription,
                Hreflangs = hreflangs
            };
        }

        private async Task<PageUnifiedVm> BuildDetailByHitAsync(
    int langId, string langCode,
    ContentItem ci, ContentItemTranslation tr,
    CancellationToken ct)
        {
            // 1) Hreflangs
            var hreflangs = await (from t in _trRepo.DataSet
                                   where !t.IsDeleted && t.IsActive && t.ContentItemId == ci.Id
                                   join la in _langRepo.DataSet on t.LanguageId equals la.Id
                                   where !la.IsDeleted && la.IsActive
                                   orderby la.Code
                                   select new HreflangVm
                                   {
                                       Lang = la.Code,
                                       Slug = t.Slug ?? "",
                                       Url = "" // Controller'da absolut üretilebilir
                                   })
                                  .ToListAsync(ct);

            // 2) Gallery (sayfa seviyesi)
            var gallery = await (from m in _mediaRepo.DataSet
                                 where !m.IsDeleted && m.IsActive
                                       && m.OwnerType == MediaOwnerType.Content
                                       && m.OwnerId == ci.Id
                                 orderby m.SortOrder, m.Id
                                 select new MediaVm
                                 {
                                     Id = m.Id,
                                     Url = m.Url,
                                     SortOrder = m.SortOrder,
                                     Alt = _mediaTrRepo.DataSet
                                            .Where(t => !t.IsDeleted && t.IsActive && t.ContentMediaId == m.Id && t.LanguageId == langId)
                                            .Select(t => t.Alt).FirstOrDefault(),
                                     Caption = _mediaTrRepo.DataSet
                                            .Where(t => !t.IsDeleted && t.IsActive && t.ContentMediaId == m.Id && t.LanguageId == langId)
                                            .Select(t => t.Caption).FirstOrDefault()
                                 })
                                .ToListAsync(ct);

            // 3) Blocks (OwnerType=Content, OwnerId=pageId)
            var rawBlocks = await (from b in _contentRepo.DataSet
                                   where !b.IsDeleted && b.IsActive
                                      && b.Type == ContentItemType.Block
                                      && b.OwnerType == ContentOwnerType.Content
                                      && b.OwnerId == ci.Id
                                   orderby b.SortOrder, b.Id
                                   select new
                                   {
                                       b.Id,
                                       b.BlockTemplate,
                                       b.SortOrder,
                                       T = _trRepo.DataSet
                                           .Where(t => !t.IsDeleted && t.IsActive && t.ContentItemId == b.Id && t.LanguageId == langId)
                                           .Select(t => new
                                           {
                                               t.Title,
                                               t.Summary,
                                               t.Body,
                                               t.Image,
                                               t.ButtonText,
                                               t.ButtonUrl,
                                               t.JsonData
                                           })
                                           .FirstOrDefault()
                                   })
                                  .ToListAsync(ct);

            var blocks = new List<BlockVm>();

            foreach (var x in rawBlocks)
            {
                var tplName = x.BlockTemplate.HasValue
                    ? x.BlockTemplate.Value.ToString() // "Hero","RichText","IncludeSnippet","RoomList","CampaignList","PageList",...
                    : "Custom";

                var vm = new BlockVm
                {
                    Id = x.Id,
                    Template = tplName,
                    Lang = langCode,
                    SortOrder = x.SortOrder,
                    Title = x.T?.Title,
                    Summary = x.T?.Summary,
                    Body = x.T?.Body,
                    Image = x.T?.Image,
                    ButtonText = x.T?.ButtonText,
                    ButtonUrl = x.T?.ButtonUrl,
                    JsonData = x.T?.JsonData
                };

                // --- Template bazlı zenginleştirme (materialize sonrası JSON parse) ---
                if (tplName == nameof(BlockTemplate.IncludeSnippet))
                {
                    var code = ReadJsonString(vm.JsonData, "snippetCode");
                    if (!string.IsNullOrWhiteSpace(code))
                    {
                        var sn = await (from s in _contentRepo.DataSet
                                        where !s.IsDeleted && s.IsActive && s.Type == ContentItemType.Snippet && s.Code == code
                                        join t in _trRepo.DataSet on s.Id equals t.ContentItemId
                                        where !t.IsDeleted && t.IsActive && t.LanguageId == langId
                                        select new { t.Title, t.Body })
                                       .FirstOrDefaultAsync(ct);

                        vm.SnippetTitle = sn?.Title;
                        vm.SnippetBody = sn?.Body;
                    }
                }
                else if (tplName == nameof(BlockTemplate.RoomList))
                {
                    var ids = ReadJsonIntArray(vm.JsonData, "pageIds");
                    var q = from p in _contentRepo.DataSet
                            where !p.IsDeleted && p.IsActive && p.Type == ContentItemType.Page
                            join t in _trRepo.DataSet on p.Id equals t.ContentItemId
                            where !t.IsDeleted && t.IsActive && t.LanguageId == langId
                            select new { p, t };

                    if (ids?.Any() == true) q = q.Where(z => ids.Contains(z.p.Id));

                    vm.Rooms = await q.OrderBy(z => z.p.SortOrder).ThenBy(z => z.p.Id)
                        .Select(z => new PageCardVm
                        {
                            Id = z.p.Id,
                            Slug = z.t.Slug ?? "",
                            Title = z.t.Title ?? "",
                            Summary = z.t.Summary,
                            Image = z.t.Image,
                            Url = BuildRelativeUrl(langCode, z.t.Slug ?? "")
                        })
                        .Take(ids?.Any() == true ? int.MaxValue : 12)
                        .ToListAsync(ct);
                }
                else if (tplName == nameof(BlockTemplate.CampaignList))
                {
                    var ids = ReadJsonIntArray(vm.JsonData, "pageIds");
                    var q = from p in _contentRepo.DataSet
                            where !p.IsDeleted && p.IsActive && p.Type == ContentItemType.Page
                            join t in _trRepo.DataSet on p.Id equals t.ContentItemId
                            where !t.IsDeleted && t.IsActive && t.LanguageId == langId
                            select new { p, t };

                    if (ids?.Any() == true) q = q.Where(z => ids.Contains(z.p.Id));

                    vm.Campaigns = await q.OrderBy(z => z.p.SortOrder).ThenBy(z => z.p.Id)
                        .Select(z => new CampaignCardVm
                        {
                            Id = z.p.Id,
                            Slug = z.t.Slug ?? "",
                            Title = z.t.Title ?? "",
                            Summary = z.t.Summary,
                            Image = z.t.Image,
                            Url = BuildRelativeUrl(langCode, z.t.Slug ?? ""),
                            Badge = ReadJsonString(z.t.JsonData, "badge"),
                            ValidFrom = ReadJsonString(z.t.JsonData, "validFrom"),
                            ValidTo = ReadJsonString(z.t.JsonData, "validTo"),
                            PriceFrom = ReadJsonDecimal(z.t.JsonData, "priceFrom"),
                            Currency = ReadJsonString(z.t.JsonData, "currency")
                        })
                        .Take(ids?.Any() == true ? int.MaxValue : 12)
                        .ToListAsync(ct);
                }
                else if (tplName == nameof(BlockTemplate.PageList))
                {
                    var ids = ReadJsonIntArray(vm.JsonData, "pageIds");
                    var slugs = ReadJsonStringArray(vm.JsonData, "slugs");
                    var take = ReadJsonInt(vm.JsonData, "take") ?? 12;
                    var order = ReadJsonString(vm.JsonData, "order") ?? "sort_asc"; // publish_desc/sort_desc/sort_asc

                    var q = from p in _contentRepo.DataSet
                            where !p.IsDeleted && p.IsActive && p.Type == ContentItemType.Page
                            join t in _trRepo.DataSet on p.Id equals t.ContentItemId
                            where !t.IsDeleted && t.IsActive && t.LanguageId == langId
                            select new { p, t };

                    if (ids?.Any() == true) q = q.Where(z => ids.Contains(z.p.Id));
                    if (slugs?.Any() == true) q = q.Where(z => slugs.Contains(z.t.Slug!));

                    q = order switch
                    {
                        "publish_desc" => q.OrderByDescending(z => z.p.PublishAtUtc).ThenBy(z => z.p.SortOrder),
                        "sort_desc" => q.OrderByDescending(z => z.p.SortOrder).ThenByDescending(z => z.p.Id),
                        _ => q.OrderBy(z => z.p.SortOrder).ThenBy(z => z.p.Id)
                    };

                    vm.Pages = await q.Take(take)
                        .Select(z => new PageCardVm
                        {
                            Id = z.p.Id,
                            Slug = z.t.Slug ?? "",
                            Title = z.t.Title ?? "",
                            Summary = z.t.Summary,
                            Image = z.t.Image,
                            Url = BuildRelativeUrl(langCode, z.t.Slug ?? "")
                        })
                        .ToListAsync(ct);
                }

                blocks.Add(vm);
            }

            // 4) Detail VM
            var detail = new PageUnifiedVm
            {
                Type = "detail",
                ContentType = GuessContentType(tr, ci), // "rooms" | "campaigns" | "page" (istersen sabit "page" yap)
                Id = ci.Id,
                Lang = langCode,
                Slug = tr.Slug,
                Title = tr.Title,
                Summary = tr.Summary,
                Body = tr.Body,
                Image = tr.Image,
                MetaTitle = string.IsNullOrWhiteSpace(tr.MetaTitle) ? tr.Title : tr.MetaTitle,
                MetaDescription = tr.MetaDescription,
                OgImage = tr.OgImage,
                Hreflangs = hreflangs,
                Gallery = gallery,
                Blocks = blocks
            };

            return detail;

        
        }
        // ---- Helpers (serviste zaten varsa bunları kullan/çıkar) ----
        private static string BuildRelativeUrl(string lCode, string s) => $"/{lCode}/{s}";

        private static string? ReadJsonString(string? json, string key)
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            try
            {
                using var d = System.Text.Json.JsonDocument.Parse(json);
                return d.RootElement.TryGetProperty(key, out var el) ? el.GetString() : null;
            }
            catch { return null; }
        }

        private static List<int>? ReadJsonIntArray(string? json, string key)
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            try
            {
                using var d = System.Text.Json.JsonDocument.Parse(json);
                if (!d.RootElement.TryGetProperty(key, out var arr) || arr.ValueKind != System.Text.Json.JsonValueKind.Array) return null;
                var list = new List<int>();
                foreach (var x in arr.EnumerateArray()) if (x.TryGetInt32(out var v)) list.Add(v);
                return list;
            }
            catch { return null; }
        }

        private static List<string>? ReadJsonStringArray(string? json, string key)
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            try
            {
                using var d = System.Text.Json.JsonDocument.Parse(json);
                if (!d.RootElement.TryGetProperty(key, out var arr) || arr.ValueKind != System.Text.Json.JsonValueKind.Array) return null;
                var list = new List<string>();
                foreach (var x in arr.EnumerateArray()) if (x.ValueKind == System.Text.Json.JsonValueKind.String) list.Add(x.GetString()!);
                return list;
            }
            catch { return null; }
        }

        private static int? ReadJsonInt(string? json, string key)
        {
            var s = ReadJsonString(json, key);
            return int.TryParse(s, out var v) ? v : null;
        }

        private static decimal? ReadJsonDecimal(string? json, string key)
        {
            var s = ReadJsonString(json, key);
            if (string.IsNullOrWhiteSpace(s)) return null;
            return decimal.TryParse(s, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var d) ? d : null;
        }

        private static string GuessContentType(ContentItemTranslation t, ContentItem c)
        {
            // Basit tahmin – istersen her zaman "page" döndür.
            var slugLow = (t.Slug ?? "").ToLowerInvariant();
            if (slugLow.Contains("room") || slugLow.Contains("oda")) return "rooms";
            if (slugLow.Contains("campaign") || slugLow.Contains("rezervasyon")) return "campaigns";
            return "page";
        }

    }

}
