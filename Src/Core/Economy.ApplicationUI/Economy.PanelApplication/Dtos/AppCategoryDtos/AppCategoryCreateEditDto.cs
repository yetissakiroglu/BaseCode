using Economy.Domain.Enums;
using Economy.Panel.Application.Dtos.AppCategoryDtos.CategoryTranslationDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Panel.Application.Dtos.AppCategoryDtos
{
    public class AppCategoryCreateEditDto
    {
        public int Id { get; set; }
        public ContentType ContentType { get; set; }
        public int? ParentCategoryId { get; set; }

        public List<AppCategoryTranslationCreateEditDto> Translations { get; set; } = new();
    }
}
