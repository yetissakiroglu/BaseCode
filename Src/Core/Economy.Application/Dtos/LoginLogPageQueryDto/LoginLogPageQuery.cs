namespace Economy.Application.Dtos.LoginLogPageQueryDto
{
    public class LoginLogPageQuery
    {
        public DateTime? DateFrom { get; set; }      // yerel tarih (UI: <input type="date">)
        public DateTime? DateTo { get; set; }
        public string? UserName { get; set; }
        public bool? Succeeded { get; set; }         // null=Hepsi, true=Başarılı, false=Başarısız
        public int? StatusCode { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 25;
    }

    public class LoginLogRow
    {
        public int Id { get; set; }
        public DateTime CreatedUtc { get; set; }
        public string? UserName { get; set; }
        public bool Succeeded { get; set; }
        public int? StatusCode { get; set; }
        public string? Message { get; set; }
        public string? IpAddress { get; set; }
        public string? RequestPath { get; set; }
        public string? UserAgent { get; set; }
        public string? CorrelationId { get; set; }
    }

    public class LoginLogPageViewModel
    {
        public LoginLogPageQuery Q { get; set; } = new();
        public List<LoginLogRow> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)Math.Max(0, TotalCount) / Math.Max(1, PageSize));
    }

}
