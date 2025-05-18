using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Panel.Application.Dtos.AppSlideDtos.SlideTranslationDtos
{
    public class AppSlideLanguageCreateDto
    {
        public int AppSlideId { get; set; }
        public int AppLanguageId { get; set; }
        public string Title { get; set; }
        public string? Content { get; set; }
        public bool IsExternal { get; set; }
        public string? ButtonText { get; set; }
        public string? ButtonUrl { get; set; }
        public string? ButtonIcon { get; set; }
    }
}
