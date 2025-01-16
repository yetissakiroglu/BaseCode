using Economy.Domain.BaseEntities;

namespace Economy.Persistence.Repositories
{
    public static class IsDeletedFilter
    {
        public static IQueryable<T> ApplyIsDeletedFalseFilter<T>(this IQueryable<T> query, bool isApplyFilter = true) where T : class, ISoftDelete
		{
            if (isApplyFilter)
            {
                return query.Where(x => x.IsDeleted == false);
            }

            return query;
        }

        public static IQueryable<T> ApplyIsDeletedFilter<T>(this IQueryable<T> query, bool isApplyFilter = true) where T : class, ISoftDelete
        {
            if (isApplyFilter)
            {
                return query.Where(x => x.IsDeleted == true);
            }

            return query;
        }

    }
}
