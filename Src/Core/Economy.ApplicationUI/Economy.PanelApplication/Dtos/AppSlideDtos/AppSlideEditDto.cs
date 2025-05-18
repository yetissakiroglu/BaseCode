using Economy.Panel.Application.Dtos.AppSlideDtos.SlideTranslationDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Panel.Application.Dtos.AppSlideDtos
{
    public class AppSlideEditDto
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public string? ThumbnailBase64 { get; set; }
        public string? ThumbnailMobilBase64 { get; set; }
        public List<AppSlideLanguageDto> Translations { get; set; }

    }
}
