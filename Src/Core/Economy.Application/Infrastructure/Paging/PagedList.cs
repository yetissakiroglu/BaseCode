namespace Economy.Application.Infrastructure.Paging
{
	public class PagedList<T> : List<T>, IPagedList<T>
    {
        public PagedList(IList<T> source, int pageNumber, int pageSize)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
            TotalItemCount = source.Count();

            if (pageSize > 0)
            {
                TotalPages = (int)Math.Ceiling(TotalItemCount / (double)pageSize);
                PageNumber = Math.Max(pageNumber, 1);
                PageNumber = Math.Min(PageNumber, TotalPages);

                AddRange(source.Skip((PageNumber - 1) * pageSize).Take(pageSize));
            }
            else
            {
                TotalPages = 1;
                AddRange(source);
            }
        }

        public int PageNumber { get; }
        public int PageSize { get; }
        public int TotalItemCount { get; }
        public int TotalPages { get; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

        public int GetIndex(int itemIndex)
        {
            return (PageNumber - 1) * PageSize + itemIndex + 1;
        }
    }
}
