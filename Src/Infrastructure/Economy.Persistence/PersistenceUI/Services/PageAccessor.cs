using Economy.Application.ApplicationUI.Interfaces;
using Economy.Core.Enums;
using Economy.Core.Interfaces;
using Economy.Domain.Entites.EntityAppLanguage;
using Economy.Domain.Entites.EntityAppNewPages;
using Economy.UI.Models.PageDtos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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
        private static string Join2(string lang, string slug) => "/" + lang + "/" + slug;
        private static string Join3(string lang, string parent, string slug) => "/" + lang + "/" + parent + "/" + slug;

        // Belirli sayfaların parent slug’larını (mevcut dilde) tek seferde topla.
        private async Task<Dictionary<int, string>> GetParentSlugMapAsync(
            int langId, IEnumerable<int?> ownerIds, CancellationToken ct)
        {
            var ids = ownerIds.Where(x => x.HasValue).Select(x => x!.Value).Distinct().ToList();
            if (ids.Count == 0) return new Dictionary<int, string>();

            return await _trRepo.DataSet
                .Where(t => !t.IsDeleted && t.IsActive && t.LanguageId == langId && ids.Contains(t.ContentItemId))
                .GroupBy(t => t.ContentItemId)
                .Select(g => new { Id = g.Key, Slug = g.Select(x => x.Slug).FirstOrDefault() })
                .ToDictionaryAsync(x => x.Id, x => x.Slug ?? "", ct);
        }

        private async Task<Dictionary<int, string>> GetParentTitleMapAsync(
    int langId, IEnumerable<int?> ownerIds, CancellationToken ct)
        {
            return await (from ci in _contentRepo.DataSet
                          join tr in _trRepo.DataSet on ci.Id equals tr.ContentItemId
                          where !ci.IsDeleted && ci.IsActive
                                && !tr.IsDeleted && tr.IsActive
                                && tr.LanguageId == langId
                                && ownerIds.Contains(ci.Id)
                          select new { ci.Id, tr.Title })
                         .ToDictionaryAsync(x => x.Id, x => x.Title ?? "", ct);
        }
        // ---------------- helpers ----------------
        private async Task<PageUnifiedVm> BuildListByHeaderAsync(
        int langId, string langCode,
        ContentItem headerCi, ContentItemTranslation headerTr,
        CancellationToken ct)
        {
            var listSlug = headerTr.Slug ?? "";

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
                                   ParentSlug = listSlug,                           // <-- parent sabit
                                   Title = tr.Title ?? "",
                                   Summary = tr.Summary,
                                   Image = tr.Image,
                                   PublishAtUtc = ci.PublishAtUtc,
                                   IsActive = ci.IsActive,
                                   Url = Join3(langCode, listSlug, tr.Slug ?? "")  // <-- /lang/parent/child
                               })
                              .ToListAsync(ct);

            var hreflangs = await (from t in _trRepo.DataSet
                                   where !t.IsDeleted && t.IsActive && t.ContentItemId == headerCi.Id
                                   join la in _langRepo.DataSet on t.LanguageId equals la.Id
                                   where !la.IsDeleted && la.IsActive
                                   orderby la.Code
                                   select new HreflangVm
                                   {
                                       Lang = la.Code,
                                       Slug = t.Slug ?? "",
                                       Url = Join2(la.Code, t.Slug ?? "")
                                   })
                                  .ToListAsync(ct);

            return new PageUnifiedVm
            {
                Type = "list",
                ContentType = (headerTr.Slug ?? "list").ToLowerInvariant(), // istersen "rooms" sabitle
                Id = headerCi.Id,
                Lang = langCode,
                Slug = headerTr.Slug,
                ParentSlug = null,
                ParentTitle = null,
                Url = Join2(langCode, listSlug),

                Title = string.IsNullOrWhiteSpace(headerTr.Title) ? headerTr.Slug : headerTr.Title,
                Summary = headerTr.Summary,
                Body = headerTr.Body,
                Image = headerTr.Image,

                MetaTitle = string.IsNullOrWhiteSpace(headerTr.MetaTitle) ? headerTr.Title : headerTr.MetaTitle,
                MetaDescription = headerTr.MetaDescription,
                Hreflangs = hreflangs,
                Items = items
            };
        }


        private async Task<PageUnifiedVm> BuildDetailByHitAsync(
       int langId, string langCode,
       ContentItem ci, ContentItemTranslation tr,
       CancellationToken ct)
        {
            // (1) Parent slug + title (önce aynı dil, yoksa herhangi dil)
            string? parentSlug = null;
            string? parentTitle = null;

            if (ci.OwnerType == ContentOwnerType.Content && ci.OwnerId.HasValue)
            {
                var parentTrs = await _trRepo.DataSet
                    .Where(x => !x.IsDeleted && x.IsActive && x.ContentItemId == ci.OwnerId.Value)
                    .OrderBy(x => x.LanguageId)
                    .Select(x => new { x.LanguageId, x.Slug, x.Title })
                    .ToListAsync(ct);

                var same = parentTrs.FirstOrDefault(x => x.LanguageId == langId);
                if (same is not null)
                {
                    parentSlug = same.Slug;
                    parentTitle = same.Title;
                }
                else if (parentTrs.Count > 0)
                {
                    parentSlug = parentTrs[0].Slug;
                    parentTitle = parentTrs[0].Title;
                }
            }

            // (2) Hreflangs (parent’ın aynı dil slug’ını da ekle)
            var hreflangs = await (from t in _trRepo.DataSet
                                   where !t.IsDeleted && t.IsActive && t.ContentItemId == ci.Id
                                   join la in _langRepo.DataSet on t.LanguageId equals la.Id
                                   where !la.IsDeleted && la.IsActive
                                   join pt in _trRepo.DataSet
                                        on new { P = (ci.OwnerId ?? 0), L = t.LanguageId }
                                        equals new { P = pt.ContentItemId, L = pt.LanguageId }
                                        into ptx
                                   from pt in ptx.DefaultIfEmpty()
                                   orderby la.Code
                                   select new HreflangVm
                                   {
                                       Lang = la.Code,
                                       Slug = t.Slug ?? "",
                                       Url = (pt != null && !string.IsNullOrWhiteSpace(pt.Slug))
                                           ? Join3(la.Code, pt.Slug!, t.Slug ?? "")
                                           : Join2(la.Code, t.Slug ?? "")
                                   })
                                  .ToListAsync(ct);

            // (3) Gallery
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

            // (4) Blocks — PageList kartları için parent’lı URL’ler
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
                var tplName = x.BlockTemplate.HasValue ? x.BlockTemplate.Value.ToString() : "Custom";
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

                if (tplName == nameof(BlockTemplate.IncludeSnippet))
                {
                    var code = ReadJsonString(vm.JsonData, "snippetCode");
                    if (!string.IsNullOrWhiteSpace(code))
                    {
                        var sn = await (from s in _contentRepo.DataSet
                                        where !s.IsDeleted && s.IsActive && s.Type == ContentItemType.Snippet && s.Code == code
                                        join ttr in _trRepo.DataSet on s.Id equals ttr.ContentItemId
                                        where !ttr.IsDeleted && ttr.IsActive && ttr.LanguageId == langId
                                        select new { ttr.Title, ttr.Body })
                                       .FirstOrDefaultAsync(ct);
                        vm.SnippetTitle = sn?.Title;
                        vm.SnippetBody = sn?.Body;
                    }
                }
                else if (tplName == nameof(BlockTemplate.PageList)
                      || tplName == nameof(BlockTemplate.RoomList)
                      || tplName == nameof(BlockTemplate.CampaignList))
                {
                    var ids = ReadJsonIntArray(vm.JsonData, "pageIds");
                    var slugs = ReadJsonStringArray(vm.JsonData, "slugs");
                    var take = ReadJsonInt(vm.JsonData, "take") ?? 12;

                    var q = from p in _contentRepo.DataSet
                            where !p.IsDeleted && p.IsActive && p.Type == ContentItemType.Page
                            join ttr in _trRepo.DataSet on p.Id equals ttr.ContentItemId
                            where !ttr.IsDeleted && ttr.IsActive && ttr.LanguageId == langId
                            select new { p, ttr };

                    if (ids?.Any() == true) q = q.Where(z => ids.Contains(z.p.Id));
                    if (slugs?.Any() == true) q = q.Where(z => slugs.Contains(z.ttr.Slug!));

                    var list = await q
                        .OrderBy(z => z.p.SortOrder).ThenBy(z => z.p.Id)
                        .Take(take)
                        .ToListAsync(ct);

                    // Parent slug’ları tek seferde çek
                    var parentSlugMap = await GetParentSlugMapAsync(langId, list.Select(z => z.p.OwnerId), ct);
                    var parentTitleMap = await GetParentTitleMapAsync(langId, list.Select(z => z.p.OwnerId), ct);

                    vm.Pages = list.Select(z =>
                    {
                        parentSlugMap.TryGetValue(z.p.OwnerId ?? 0, out var pSlug);
                        parentTitleMap.TryGetValue(z.p.OwnerId ?? 0, out var pTitle);

                        pSlug = pSlug ?? "";
                        pTitle = pTitle ?? "";

                        return new PageCardVm
                        {
                            Id = z.p.Id,
                            Slug = z.ttr.Slug ?? "",
                            ParentSlug = string.IsNullOrWhiteSpace(pSlug) ? null : pSlug,
                            ParentTitle = string.IsNullOrWhiteSpace(pTitle) ? null : pTitle,
                            Title = z.ttr.Title ?? "",
                            Summary = z.ttr.Summary,
                            Image = z.ttr.Image,
                            Url = !string.IsNullOrWhiteSpace(pSlug)
                                ? Join3(langCode, pSlug, z.ttr.Slug ?? "")
                                : Join2(langCode, z.ttr.Slug ?? "")
                        };
                    }).ToList();
                }

                blocks.Add(vm);
            }

            // (5) Kendi URL
            var selfUrl = !string.IsNullOrWhiteSpace(parentSlug)
                ? Join3(langCode, parentSlug!, tr.Slug ?? "")
                : Join2(langCode, tr.Slug ?? "");

            // (6) VM (items = null çünkü detail)
            return new PageUnifiedVm
            {
                Type = "detail",
                ContentType = GuessContentType(tr, ci), // istersen sabit "rooms" ver
                Id = ci.Id,
                Lang = langCode,
                Slug = tr.Slug,
                ParentSlug = parentSlug,
                ParentTitle = parentTitle,
                Url = selfUrl,

                Title = tr.Title,
                Summary = tr.Summary,
                Body = tr.Body,
                Image = tr.Image,
                MetaTitle = string.IsNullOrWhiteSpace(tr.MetaTitle) ? tr.Title : tr.MetaTitle,
                MetaDescription = tr.MetaDescription,
                OgImage = tr.OgImage,

                Hreflangs = hreflangs,
                Gallery = gallery,
                Blocks = blocks,
                Items = null
            };
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
