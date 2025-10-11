using Economy.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.TenantUI.Dtos
{
    public class BlockGroupDto
    {
        public int? Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public BlockColumns Columns { get; set; } = BlockColumns.Three;
        public ImageMode DefaultImageMode { get; set; } = ImageMode.CoverOnly;
        public bool ShowTitle { get; set; } = true;
        public bool ShowDescription { get; set; } = true;
        public int? PageId { get; set; }
        public List<BlockItemDto> Items { get; set; } = new();
    }
}
