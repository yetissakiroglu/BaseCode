using Economy.Domain.Entites.EntityAppCategories;
using Economy.Domain.Entites.EntityCategories;
using Economy.Panel.Application.Dtos.AppCategoryDtos;
using Economy.Panel.Application.Dtos.AppCategoryDtos.CategoryTranslationDtos;

namespace Economy.Panel.Persistence.Extensions
{
    public static class CategoryMapper
    {
        public static void MapToEntity(AppCategory entity, AppCategoryCreateEditDto model)
        {
            entity.ContentType = model.ContentType;
            entity.ParentCategoryId = model.ParentCategoryId;

            entity.Translations = model.Translations?.Select(t => new AppCategoryTranslation
            {
                Id = t.Id,
                AppLanguageId = t.AppLanguageId,
                Title = t.Title,
                ShortDescription = t.ShortDescription,
                Content = t.Content,
                MetaTitle = t.MetaTitle,
                MetaDescription = t.MetaDescription,
                Url = t.Url
            }).ToList() ?? new List<AppCategoryTranslation>();
        }

        public static AppCategoryDto MapToDto(this AppCategory entity)
        {
            return new AppCategoryDto
            {
                Id = entity.Id,
                ContentType = entity.ContentType,
                ParentCategoryId = entity.ParentCategoryId,
                Translations = entity.Translations.Select(MapTranslationEntityToDto).ToList()
            };
        }

        public static AppCategoryDto MapSelectToDto(AppCategory entity)
        {
            return new AppCategoryDto
            {
                Id = entity.Id,
                ContentType = entity.ContentType,
                ParentCategoryId = entity.ParentCategoryId,
                SubCategories = entity.SubCategories?.Select(MapSelectToDto).ToList() ?? new List<AppCategoryDto>(),
                Translations = entity.Translations.Select(MapTranslationEntityToDto).ToList()
            };
        }

        // ------------------------
        // Yardımcı Metot
        // ------------------------

        private static AppCategoryTranslationDto MapTranslationEntityToDto(AppCategoryTranslation entity)
        {
            return new AppCategoryTranslationDto
            {
                Id = entity.Id,
                AppCategoryId = entity.AppCategoryId,
                AppLanguageId = entity.AppLanguageId,
                Title = entity.Title,
                ShortDescription = entity.ShortDescription,
                Content = entity.Content,
                MetaTitle = entity.MetaTitle,
                MetaDescription = entity.MetaDescription,
                Url = entity.Url
            };
        }
    }

}
