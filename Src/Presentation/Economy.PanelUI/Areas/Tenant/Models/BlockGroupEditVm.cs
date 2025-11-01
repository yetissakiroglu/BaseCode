using Economy.Application.TenantUI.Dtos.AppBlockDtos;
using Economy.Application.TenantUI.Dtos.AppBlockGroupDtos;

namespace Economy.Panel.UI.Areas.Tenant.Models
{
    public class BlockGroupEditVm
    {
        public int GroupId { get; set; }
        public string GroupTitle { get; set; } = "";
        public List<AppBlockMiniDto> AllBlocks { get; set; } = new();
        public List<GroupLayoutItemDto> Selected { get; set; } = new();
    }
}
