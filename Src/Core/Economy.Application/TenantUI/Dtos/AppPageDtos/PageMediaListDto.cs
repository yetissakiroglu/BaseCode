namespace Economy.Application.TenantUI.Dtos.AppPageDtos
{
    public class PageMediaListDto
    {
        public int Id { get; set; }
        public string? MediaUrl { get; set; }
        public bool IsCover { get; set; }
        public string? Alt { get; set; }
        public string? Caption { get; set; }
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
    }
}
