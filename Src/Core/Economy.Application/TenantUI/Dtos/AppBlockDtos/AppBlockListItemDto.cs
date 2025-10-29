using Economy.Core.Enums;

namespace Economy.Application.TenantUI.Dtos.AppBlockDtos
{
    public class AppBlockListItemDto
    {
        public string Tag { get; set; }
        public int Id { get; set; }
        public BlockType Type { get; set; }
        //public int SortOrder { get; set; }
        public bool IsActive { get; set; }
        //public ContentStage Stage { get; set; }
    }
}
