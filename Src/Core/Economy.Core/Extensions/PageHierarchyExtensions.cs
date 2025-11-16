namespace Economy.Core.Extensions
{
    public static class PageHierarchyExtensions
    {
        /// <summary>
        /// Parent zincirini yukarı doğru yürüyerek slug listesini toplar.
        /// Örn: en alt sayfadan başlar, [root, sub, current] şeklinde döner.
        /// maxDepth: olası sonsuz döngülere karşı güvenlik sağlar.
        /// </summary>
        public static List<string> BuildSlugPath<T>(
            this IReadOnlyDictionary<int, T> items,
            int startId,
            Func<T, int?> parentSelector,
            Func<T, string> slugSelector,
            int maxDepth = 32)
        {
            var slugs = new List<string>();
            var currentId = startId;
            var visited = new HashSet<int>();

            for (var i = 0; i < maxDepth; i++)
            {
                T item;
                if (!items.TryGetValue(currentId, out item))
                    break;

                if (!visited.Add(currentId))
                    break; // döngü varsa kır

                var slug = slugSelector(item);
                if (!string.IsNullOrWhiteSpace(slug))
                    slugs.Add(slug);

                var parentId = parentSelector(item);
                if (!parentId.HasValue)
                    break;

                currentId = parentId.Value;
            }

            slugs.Reverse(); // root -> child sırasına çevir
            return slugs;
        }
    }
}
