namespace Economy.Core.Tools
{
    public class PaginationInfo
    {
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public int TotalItems { get; init; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    }
}
