using Economy.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.TenantUI.Dtos.AppBlockDtos
{
    public class AppBlockDto
    {
        public int? Id { get; set; }
        public string? Tag { get; set; }
        public BlockType Type { get; set; }
        public bool IsActive { get; set; } = true;
        public string SharedJson { get; set; } = "{}";
        public List<AppBlockTranslationDto> Translations { get; set; } = new();
    }
}
