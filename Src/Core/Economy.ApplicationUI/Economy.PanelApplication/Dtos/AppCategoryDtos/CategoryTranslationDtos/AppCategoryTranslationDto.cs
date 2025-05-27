using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Panel.Application.Dtos.AppCategoryDtos.CategoryTranslationDtos
{
    public class AppCategoryTranslationDto
    {
        public int Id { get; set; }
        public int AppCategoryId { get; set; }
        public int AppLanguageId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string? ShortDescription { get; set; }
        public string? Content { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
    }
}
