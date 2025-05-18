using Economy.Panel.Application.Dtos.AppSlideDtos;
using Economy.Panel.Application.Dtos.AppSlideDtos.SlideTranslationDtos;
using Economy.Panel.UI.Models.SlideViewModels;

namespace Economy.Panel.UI.Extensions
{
    public static class AppSlideViewModelExtensions
    {
        public static AppSlideEditDto ToDto(this AppSlideEditViewModel model)
        {
            return new AppSlideEditDto
            {
                Id = model.Id,
                Sequence = model.Sequence,
                ThumbnailBase64 = model.ThumbnailBase64,
                ThumbnailMobilBase64 = model.ThumbnailMobilBase64,
                Translations = model.Translations.Select(t => new AppSlideLanguageDto
                {
                    Id = t.Id,
                    AppSlideId = t.AppSlideId,
                    AppLanguageId = t.AppLanguageId,
                    Title = t.Title,
                    Content = t.Content,
                    IsExternal = t.IsExternal,
                    ButtonText = t.ButtonText,
                    ButtonUrl = t.ButtonUrl,
                    ButtonIcon = t.ButtonIcon
                }).ToList()
            };
        }

        public static AppSlideCreateDto ToDto(this AppSlideCreateViewModel model)
        {
            return new AppSlideCreateDto
            {
                Sequence = model.Sequence,
                ThumbnailBase64 = model.ThumbnailBase64,
                ThumbnailMobilBase64 = model.ThumbnailMobilBase64,
                Translations = model.Translations.Select(t => new AppSlideLanguageDto
                {
                    AppLanguageId = t.AppLanguageId,
                    Title = t.Title,
                    Content = t.Content,
                    IsExternal = t.IsExternal,
                    ButtonText = t.ButtonText,
                    ButtonUrl = t.ButtonUrl,
                    ButtonIcon = t.ButtonIcon
                }).ToList()
            };
        }
    }
}
