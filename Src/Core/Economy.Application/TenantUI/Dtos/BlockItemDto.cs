using Economy.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.TenantUI.Dtos
{
    public class BlockItemDto
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string? Summary { get; set; }
        public string CoverImage { get; set; }
        public ImageMode? ImageModeOverride { get; set; }
        public List<string> Gallery { get; set; } = new();
        public LinkType LinkType { get; set; } = LinkType.None;
        public int? LinkedPageId { get; set; }
        public string? ExternalUrl { get; set; }
        public string? Target { get; set; }
        public BlockColumns? ColumnsOverride { get; set; }
        public int SortOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
    }
}
