using Economy.Core.Extensions;
using Economy.Domain.Entites.TenantEntity.EntityAppPages;

namespace Economy.Application.Extensions
{
    public static class AppPageExtensions
    {
        /// <summary>
        /// Page içinden ihtiyacımız olan minimal alanlar.
        /// </summary>
        public sealed class PageFlat
        {
            public int Id { get; set; }
            public int? ParentId { get; set; }
            public string Slug { get; set; }
            public string Title { get; set; }
        }

        /// <summary>
        /// AppPage listesinden (dil bazlı) flatten dictionary çıkarır.
        /// Id -> PageFlat
        /// </summary>
        public static IReadOnlyDictionary<int, PageFlat> ToPageFlatDictionary(
            this IEnumerable<AppPage> pages,
            int langId)
        {
            var dict = pages
                .Select(p => new PageFlat
                {
                    Id = p.Id,
                    Title = p.Translations
                        .Where(t => !t.IsDeleted && t.AppLanguageId == langId)
                        .Select(t => t.Title)
                        .FirstOrDefault(),
                    ParentId = p.AppPageId,
                    Slug = p.Translations
                        .Where(t => !t.IsDeleted && t.AppLanguageId == langId)
                        .Select(t => t.Slug)
                        .FirstOrDefault()
                })
                .ToDictionary(x => x.Id, x => x);

            return dict;
        }

        /// <summary>
        /// PageFlat dictionary ve startId'den URL segment listesi üretir.
        /// Örn: /tr/ + string.Join("/", slugList)
        /// </summary>
        public static List<string> BuildSlugPath(
            this IReadOnlyDictionary<int, PageFlat> pages,
            int startId,
            int maxDepth = 32)
        {
            return PageHierarchyExtensions.BuildSlugPath(
                pages,
                startId,
                p => p.ParentId,
                p => p.Slug,
                maxDepth);
        }
    }
}
