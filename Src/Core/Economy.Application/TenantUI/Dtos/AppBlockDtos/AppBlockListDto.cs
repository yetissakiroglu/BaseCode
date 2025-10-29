using Economy.Core.Enums;

namespace Economy.Application.TenantUI.Dtos.AppBlockDtos
{
    public class AppBlockListDto
    {
        public List<AppBlockListItemDto> Items { get; set; } = new();
        public int Page { get; set; }
        public int Size { get; set; }
        public int Total { get; set; }
        public BlockType? FilterType { get; set; }
        public string? Q { get; set; }
    }
}
