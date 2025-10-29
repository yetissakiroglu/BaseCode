using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.TenantUI.Dtos.AppBlockDtos
{
    public class AppBlockTranslationDto
    {
        public int? Id { get; set; }
        public int LanguageId { get; set; }
        public string LanguageName { get; set; } = default!;
        public string LanguageCode { get; set; } = default!;
        public string? LanguageIcon { get; set; } = default!;
        public string LocalizedJson { get; set; } = "{}";
    }
}
