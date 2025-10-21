using Economy.Core.Enums;

namespace Economy.Application.TenantUI.Dtos
{
    public class BlockGroupListDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public BlockColumns Columns { get; set; } = BlockColumns.Three;
        public bool ShowTitle { get; set; } = true;
        public bool ShowDescription { get; set; } = true;
    }
}
