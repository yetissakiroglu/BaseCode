using Economy.Domain.BaseEntities;

namespace Economy.Persistence.Repositories
{
    public static class IsDeletedFilter
    {
        public static IQueryable<T> ApplyIsDeletedFalseFilter<T>(this IQueryable<T> query, bool isApplyFilter = true) where T : class, ISoftDelete
        {
            // Filtre uygulanacaksa, sorguya 'IsDeleted' filtresi eklenir
            return isApplyFilter ? query.Where(x => !x.IsDeleted) : query;
        }

        public static IQueryable<T> ApplyIsDeletedFilter<T>(this IQueryable<T> query, bool isApplyFilter = true) where T : class, ISoftDelete
        {
            // Filtre uygulanacaksa, sorguya 'IsDeleted' filtresi eklenir
            return isApplyFilter ? query.Where(x => x.IsDeleted) : query;
        }
    }
}
