using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.TenantUI.Dtos.AppPageDtos
{
    public class PageMediaEditDto
    {
        public int? Id { get; set; }            
        public int AppPageId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsCover { get; set; } = false; 
        public int SortOrder { get; set; } = 0;
        public string? MediaUrl { get; set; }
        public List<PageMediaTranslationDto> Translations { get; set; } = new();
    }

    public class PageMediaTranslationDto
    {
        public int? Id { get; set; }
        public int AppLanguageId { get; set; }
        public string AppLanguageCode { get; set; } = default!;
        public string? AppLanguageIcon { get; set; } = default!;
        public string? Alt { get; set; }
        public string? Caption { get; set; }

    }

}
