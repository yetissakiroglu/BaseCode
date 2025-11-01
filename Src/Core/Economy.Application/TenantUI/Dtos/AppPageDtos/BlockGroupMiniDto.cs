namespace Economy.Application.TenantUI.Dtos.AppPageDtos
{
    public sealed class BlockGroupMiniDto
    {
        public int Id { get; set; }          // BlockGroup.Id
        public string Title { get; set; }    // BlockGroup.Title
        public int Columns { get; set; }     // BlockGroup.Columns (int)
        public bool IsActive { get; set; }   // BlockGroup.IsActive
        public int SortOrder { get; set; }   // PageBlock.SortOrder (sayfaya göre)
    }
}
