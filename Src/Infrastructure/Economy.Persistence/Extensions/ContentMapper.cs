using Economy.Domain.Entites.EntityAppContents.AppContents;
using Economy.Panel.Application.Dtos.AppContentDtos;
using Economy.Panel.Application.Dtos.AppContentDtos.AppContentTranslationDtos;

namespace Economy.Panel.Persistence.Extensions
{
    public static class ContentMapper
    {
        public static void MapToEntity(AppContent entity, AppContentCreateEditDto model)
        {
            entity.ContentType = model.ContentType;
            entity.WebThumbnailUrl = model.WebThumbnailUrl;
            entity.MobilThumbnailUrl = model.MobilThumbnailUrl;
            entity.AppCategoryId = model.AppCategoryId;

            entity.Translations = model.Translations?.Select(t => new AppContentTranslation
            {
                Id = t.Id,
                AppLanguageId = t.AppLanguageId,
                Title = t.Title,
                ShortDescription = t.ShortDescription,
                Content = t.Content,
                MetaTitle = t.MetaTitle,
                MetaDescription = t.MetaDescription,
                Url = t.Url,
                AppContentId = entity.Id,
                IsExternal = t.IsExternal
            }).ToList() ?? new List<AppContentTranslation>();
        }

        public static AppContentDto MapToDto(this AppContent entity)
        {
            return new AppContentDto
            {
                Id = entity.Id,
                ContentType = entity.ContentType,
                Translations = entity.Translations.Select(MapTranslationEntityToDto).ToList()
            };
        }

        public static AppContentDto MapSelectToDto(AppContent entity)
        {
            return new AppContentDto
            {
                Id = entity.Id,
                AppCategoryId = entity.AppCategoryId,
                MobilThumbnailUrl = entity.MobilThumbnailUrl,
                WebThumbnailUrl = entity.WebThumbnailUrl,
                ContentType = entity.ContentType,
                Translations = entity.Translations.Select(MapTranslationEntityToDto).ToList()
            };
        }

        // ------------------------
        // Yardımcı Metot
        // ------------------------

        private static AppContentTranslationDto MapTranslationEntityToDto(AppContentTranslation entity)
        {
            return new AppContentTranslationDto
            {
                Id = entity.Id,
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
