using Economy.Core.Enums;

namespace Economy.Application.TenantUI.Dtos.AppBlockGroupDtos
{
    public class AppBlockGroupListDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public BlockColumns Columns { get; set; } = BlockColumns.Three;
        public bool ShowTitle { get; set; } = true;
        public bool ShowDescription { get; set; } = true;
        public bool IsActive { get; set; }
        public int BlockCount { get; set; }

    }
}
