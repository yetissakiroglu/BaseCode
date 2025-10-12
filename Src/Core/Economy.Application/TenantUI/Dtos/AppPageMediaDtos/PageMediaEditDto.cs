using Economy.Application.TenantUI.Dtos.AppPageDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.TenantUI.Dtos.AppPageMediaDtos
{
    public class PageMediaEditDto
    {
        public int? Id { get; set; }                // ContentItem.Id
        public int ContentItemId { get; set; }           // Parent
        public bool IsActive { get; set; } = true;
        public bool IsCover { get; set; } = false; 
        public int SortOrder { get; set; } = 0;
        public string MediaUrl { get; set; }
        public List<PageMediaTranslationDto> Translations { get; set; } = new();
    }

    public class PageMediaTranslationDto
    {
        public int? Id { get; set; }
        public int LanguageId { get; set; }
        public string LanguageCode { get; set; } = default!;
        public string? LanguageIcon { get; set; } = default!;
        public string? Alt { get; set; }
        public string? Caption { get; set; }

    }

}
