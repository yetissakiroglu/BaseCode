namespace Economy.Core.PagingModels
{
    public interface IPagedList<T> : IList<T>
    {
        int PageNumber { get; }
        int PageSize { get; }
        int TotalItemCount { get; }
        int TotalPages { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get; }
        int GetIndex(int itemIndex);
    }
}
