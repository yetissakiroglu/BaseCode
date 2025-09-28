using Economy.Application.ApplicationUI.Interfaces;
using Economy.Core.Enums;
using Economy.Core.Interfaces;
using Economy.Domain.Entites.EntityAppLanguage;
using Economy.Domain.Entites.EntityAppNewPages;
using Economy.UI.Models.PageDtos;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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
            // 1) Dil
            var want = (lang ?? "tr").ToLowerInvariant();
            var l = await _langRepo.DataSet
                    .Where(x => !x.IsDeleted && x.IsActive && x.Code.ToLower() == want)
                    .Select(x => new { x.Id, x.Code })
                    .FirstOrDefaultAsync(ct);

            if (l is null)
            {
                l = await _langRepo.DataSet
                    .Where(x => !x.IsDeleted && x.IsActive && x.IsDefault)
                    .Select(x => new { x.Id, x.Code })
                    .FirstOrDefaultAsync(ct)
                    ?? new { Id = 1, Code = "tr" };
            }
            var langId = l.Id;
            var langCode = l.Code;

            // 2) Liste sayfaları: "rooms" ve "campaigns"
            if (string.Equals(slug, "rooms", StringComparison.OrdinalIgnoreCase))
                return await BuildListAsync(langId, langCode, "rooms", ct);

            if (string.Equals(slug, "campaigns", StringComparison.OrdinalIgnoreCase))
                return await BuildListAsync(langId, langCode, "campaigns", ct);

            // 3) Detay sayfası: slug eşleşmesi (ContentItemType.Page)
            var hit = await (from ci in _contentRepo.DataSet
                             where !ci.IsDeleted && ci.IsActive && ci.Type == ContentItemType.Page
                             join tr in _trRepo.DataSet on ci.Id equals tr.ContentItemId
                             where !tr.IsDeleted && tr.IsActive && tr.LanguageId == langId && tr.Slug == slug
                             select new { ci, tr }).FirstOrDefaultAsync(ct);

            if (hit is null) return null;

            // 4) hreflangs
            var hreflangs = await (from tr in _trRepo.DataSet
                                   where !tr.IsDeleted && tr.ContentItemId == hit.ci.Id && tr.IsActive
                                   join la in _langRepo.DataSet on tr.LanguageId equals la.Id
                                   where !la.IsDeleted && la.IsActive
                                   orderby la.Code
                                   select new HreflangVm
                                   {
                                       Lang = la.Code,
                                       Slug = tr.Slug ?? "",
                                       Url = "" // mutlak URL istersen controller içinde üret
                                   }).ToListAsync(ct);

            // 5) gallery
            var gallery = await (from m in _mediaRepo.DataSet
                                 where !m.IsDeleted && m.IsActive
                                       && m.OwnerType == MediaOwnerType.Content
                                       && m.OwnerId == hit.ci.Id
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
                                 }).ToListAsync(ct);

            // 6) blocks (ContentItem.Type=Block, OwnerType=Content, OwnerId=pageId)
            var blocks = await (from b in _contentRepo.DataSet
                                where !b.IsDeleted && b.IsActive
                                   && b.Type == ContentItemType.Block
                                   && b.OwnerType == ContentOwnerType.Content
                                   && b.OwnerId == hit.ci.Id
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
                                }).ToListAsync(ct);

            var blockVms = new List<BlockVm>();
            foreach (var x in blocks)
            {
                var tplName = TemplateToName((short)x.BlockTemplate);
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

                // Template-specific enrichment
                if (tplName == "IncludeSnippet")
                {
                    var code = ReadJsonString(vm.JsonData, "snippetCode");
                    if (!string.IsNullOrWhiteSpace(code))
                    {
                        var sn = await (from ci in _contentRepo.DataSet
                                        where !ci.IsDeleted && ci.IsActive && ci.Type == ContentItemType.Snippet && ci.Code == code
                                        join tr in _trRepo.DataSet on ci.Id equals tr.ContentItemId
                                        where !tr.IsDeleted && tr.IsActive && tr.LanguageId == langId
                                        select new { tr.Title, tr.Body })
                                       .FirstOrDefaultAsync(ct);

                        vm.SnippetTitle = sn?.Title;
                        vm.SnippetBody = sn?.Body;
                    }
                }
                else if (tplName == "RoomList")
                {
                    var ids = ReadJsonIntArray(vm.JsonData, "pageIds");
                    var q = from ci in _contentRepo.DataSet
                            where !ci.IsDeleted && ci.IsActive && ci.Type == ContentItemType.Page
                            join tr in _trRepo.DataSet on ci.Id equals tr.ContentItemId
                            where !tr.IsDeleted && tr.IsActive && tr.LanguageId == langId 

                            select new { ci, tr };

                    if (ids?.Any() == true) q = q.Where(z => ids.Contains(z.ci.Id));

                    vm.Rooms = await q.OrderBy(z => z.ci.SortOrder).ThenBy(z => z.ci.Id)
                        .Select(z => new PageCardVm
                        {
                            Id = z.ci.Id,
                            Slug = z.tr.Slug ?? "",
                            Title = z.tr.Title ?? "",
                            Summary = z.tr.Summary,
                            Image = z.tr.Image,
                            Url = BuildRelativeUrl(langCode, z.tr.Slug ?? "")
                        }).Take(12).ToListAsync(ct);
                }
                else if (tplName == "CampaignList")
                {
                    var ids = ReadJsonIntArray(vm.JsonData, "pageIds");
                    var q = from ci in _contentRepo.DataSet
                            where !ci.IsDeleted && ci.IsActive && ci.Type == ContentItemType.Page
                            join tr in _trRepo.DataSet on ci.Id equals tr.ContentItemId
                            where !tr.IsDeleted && tr.IsActive && tr.LanguageId == langId
                            select new { ci, tr };

                    if (ids?.Any() == true) q = q.Where(z => ids.Contains(z.ci.Id));

                    vm.Campaigns = await q.OrderBy(z => z.ci.SortOrder).ThenBy(z => z.ci.Id)
                        .Select(z => new CampaignCardVm
                        {
                            Id = z.ci.Id,
                            Slug = z.tr.Slug ?? "",
                            Title = z.tr.Title ?? "",
                            Summary = z.tr.Summary,
                            Image = z.tr.Image,
                            Url = BuildRelativeUrl(langCode, z.tr.Slug ?? ""),
                            Badge = ReadJsonString(z.tr.JsonData, "badge"),
                            ValidFrom = ReadJsonString(z.tr.JsonData, "validFrom"),
                            ValidTo = ReadJsonString(z.tr.JsonData, "validTo"),
                            PriceFrom = ReadJsonDecimal(z.tr.JsonData, "priceFrom"),
                            Currency = ReadJsonString(z.tr.JsonData, "currency")
                        }).Take(12).ToListAsync(ct);
                }
                else if (tplName == "PageList")
                {
                    var ids = ReadJsonIntArray(vm.JsonData, "pageIds");
                    var slugs = ReadJsonStringArray(vm.JsonData, "slugs");
                    var cat = ReadJsonString(vm.JsonData, "category");
                    var take = ReadJsonInt(vm.JsonData, "take") ?? 12;
                    var order = ReadJsonString(vm.JsonData, "order") ?? "sort_asc";

                    var q = from ci in _contentRepo.DataSet
                            where !ci.IsDeleted && ci.IsActive && ci.Type == ContentItemType.Page
                            join tr in _trRepo.DataSet on ci.Id equals tr.ContentItemId
                            where !tr.IsDeleted && tr.IsActive && tr.LanguageId == langId
                            select new { ci, tr };

                    if (ids?.Any() == true) q = q.Where(z => ids.Contains(z.ci.Id));
                    if (slugs?.Any() == true) q = q.Where(z => slugs.Contains(z.tr.Slug!));
                    if (!string.IsNullOrWhiteSpace(cat))
                        q = q.Where(z => (ReadJsonString(z.tr.JsonData, "category") ?? "") == cat);

                    q = order switch
                    {
                        "publish_desc" => q.OrderByDescending(z => z.ci.PublishAtUtc).ThenBy(z => z.ci.SortOrder),
                        "sort_desc" => q.OrderByDescending(z => z.ci.SortOrder).ThenByDescending(z => z.ci.Id),
                        _ => q.OrderBy(z => z.ci.SortOrder).ThenBy(z => z.ci.Id)
                    };

                    vm.Pages = await q.Take(take)
                        .Select(z => new PageCardVm
                        {
                            Id = z.ci.Id,
                            Slug = z.tr.Slug ?? "",
                            Title = z.tr.Title ?? "",
                            Summary = z.tr.Summary,
                            Image = z.tr.Image,
                            Url = BuildRelativeUrl(langCode, z.tr.Slug ?? "")
                        }).ToListAsync(ct);
                }

                blockVms.Add(vm);
            }

            // 7) Detail VM
            var detailVm = new PageUnifiedVm
            {
                Type = "detail",
                ContentType = GuessContentType(hit.tr, hit.ci), // "rooms" | "campaigns" | "page"
                Id = hit.ci.Id,
                Lang = langCode,
                Slug = hit.tr.Slug,
                Title = hit.tr.Title,
                Summary = hit.tr.Summary,
                Body = hit.tr.Body,
                Image = hit.tr.Image,
                MetaTitle = string.IsNullOrWhiteSpace(hit.tr.MetaTitle) ? hit.tr.Title : hit.tr.MetaTitle,
                MetaDescription = hit.tr.MetaDescription,
                OgImage = hit.tr.OgImage,
                Hreflangs = hreflangs,
                Gallery = gallery,
                Blocks = blockVms
            };

            return detailVm;
        }

        // ---------------- helpers ----------------

        private static string TemplateToName(short? tpl) => tpl switch
        {
            (short)BlockTemplate.RichText => "RichText",
            (short)BlockTemplate.ImageText => "ImageText",
            (short)BlockTemplate.Hero => "Hero",
            (short)BlockTemplate.Gallery => "Gallery",
            (short)BlockTemplate.IncludeSnippet => "IncludeSnippet",
            (short)BlockTemplate.PageList => "PageList",
            (short)BlockTemplate.RoomList => "RoomList",
            (short)BlockTemplate.CampaignList => "CampaignList",
            _ => "Custom"
        };

        private static string GuessContentType(ContentItemTranslation tr, ContentItem ci)
        {
            // İstersen tr.JsonData -> category ile karar verebilirsin.
            // Basit yaklaşım: "rooms/campaigns" slug eşleşmesi veya category=Campaign/Room
            var slug = tr.Slug?.ToLowerInvariant() ?? "";
            var cat = ReadJsonString(tr.JsonData, "category")?.ToLowerInvariant();

            if (cat == "campaign" || slug.Contains("rezervasyon") || slug.Contains("campaign")) return "campaigns";
            if (cat == "room" || slug.Contains("oda") || slug.Contains("room")) return "rooms";
            return "page";
        }

        private static string BuildRelativeUrl(string langCode, string slug) => $"/{langCode}/{slug}";

        private static string? ReadJsonString(string? json, string key)
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            try
            {
                using var doc = JsonDocument.Parse(json);
                return doc.RootElement.TryGetProperty(key, out var el) ? el.GetString() : null;
            }
            catch { return null; }
        }

        private static List<int>? ReadJsonIntArray(string? json, string key)
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            try
            {
                using var doc = JsonDocument.Parse(json);
                if (!doc.RootElement.TryGetProperty(key, out var el) || el.ValueKind != JsonValueKind.Array) return null;
                var list = new List<int>();
                foreach (var x in el.EnumerateArray())
                    if (x.TryGetInt32(out var v)) list.Add(v);
                return list;
            }
            catch { return null; }
        }

        private static List<string>? ReadJsonStringArray(string? json, string key)
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            try
            {
                using var doc = JsonDocument.Parse(json);
                if (!doc.RootElement.TryGetProperty(key, out var el) || el.ValueKind != JsonValueKind.Array) return null;
                var list = new List<string>();
                foreach (var x in el.EnumerateArray())
                    if (x.ValueKind == JsonValueKind.String) list.Add(x.GetString()!);
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
            return decimal.TryParse(s, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var d)
                 ? d : null;
        }
        private async Task<PageUnifiedVm> BuildListAsync(int langId, string langCode, string contentType, CancellationToken ct)
        {
            // contentType = "rooms" | "campaigns"
            var listSlug = contentType == "rooms" ? "rooms" : "campaigns";

            // 1) Liste sayfasını bul (meta/hreflang ve üst alanlar için)
            var listHeader = await (from ci in _contentRepo.DataSet
                                    where !ci.IsDeleted && ci.IsActive && ci.Type == ContentItemType.Page
                                    join tr in _trRepo.DataSet on ci.Id equals tr.ContentItemId
                                    where !tr.IsDeleted && tr.IsActive && tr.LanguageId == langId && tr.Slug == listSlug
                                    select new { ci, tr })
                                   .FirstOrDefaultAsync(ct);

            // Liste sayfası yoksa boş liste dön
            if (listHeader is null)
            {
                return new PageUnifiedVm
                {
                    Type = "list",
                    ContentType = contentType,
                    Items = new List<PageListItemVm>()
                };
            }

            // 2) Üyelik: OwnerType=Content & OwnerId = liste sayfasının Id’si
            var items = await (from ci in _contentRepo.DataSet
                               where !ci.IsDeleted && ci.IsActive
                                     && ci.Type == ContentItemType.Page
                                     && ci.OwnerType == ContentOwnerType.Content
                                     && ci.OwnerId == listHeader.ci.Id
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

            // 3) VM (liste sayfasının kendi alanlarını da doldur)
            var vm = new PageUnifiedVm
            {
                Type = "list",
                ContentType = contentType,
                Items = items,

                Id = listHeader.ci.Id,
                Lang = langCode,
                Slug = listHeader.tr.Slug,
                Title = listHeader.tr.Title,
                Summary = listHeader.tr.Summary,
                Body = listHeader.tr.Body,
                Image = listHeader.tr.Image,

                MetaTitle = string.IsNullOrWhiteSpace(listHeader.tr.MetaTitle)
                            ? listHeader.tr.Title
                            : listHeader.tr.MetaTitle,
                MetaDescription = listHeader.tr.MetaDescription
            };

            // 4) hreflangs
            vm.Hreflangs = await (from t in _trRepo.DataSet
                                  where !t.IsDeleted && t.IsActive && t.ContentItemId == listHeader.ci.Id
                                  join la in _langRepo.DataSet on t.LanguageId equals la.Id
                                  where !la.IsDeleted && la.IsActive
                                  orderby la.Code
                                  select new HreflangVm
                                  {
                                      Lang = la.Code,
                                      Slug = t.Slug ?? "",
                                      Url = "" // absolut URL istersen controller’da üret
                                  })
                                 .ToListAsync(ct);

            return vm;
        }

    }

}
