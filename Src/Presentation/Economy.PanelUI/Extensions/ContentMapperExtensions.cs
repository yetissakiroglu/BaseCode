using Economy.Panel.Application.Dtos.AppCategoryDtos;
using Economy.Panel.Application.Dtos.AppCategoryDtos.CategoryTranslationDtos;
using Economy.Panel.Application.Dtos.AppContentDtos;
using Economy.Panel.Application.Dtos.AppContentDtos.AppContentTranslationDtos;
using Economy.Panel.Application.Dtos.AppLanguageDtos;
using Economy.Panel.UI.Models.CategoryViewModels;
using Economy.Panel.UI.Models.CategoryViewModels.AppCategoryTranslationViewModels;
using Economy.Panel.UI.Models.ContentViewModels;
using Economy.Panel.UI.Models.ContentViewModels.AppContentTranslationViewModels;

namespace Economy.Panel.UI.Extensions
{
    /// <summary>
    /// Extension methods for mapping and creating ViewModel and DTO objects related to AppSlide.
    /// </summary>
    public static class ContentMapperExtensions
    {
        public static List<AppContentListViewModel> MapToListViewModel(this IEnumerable<AppContentDto> slides,IEnumerable<AppLanguageDto> languages)
        {
            return slides.Select(slide => new AppContentListViewModel
            {
                Id = slide.Id,
                ContentType =slide.ContentType,
                Translations = languages.Select(lang => MapToTranslationViewModel(lang, slide.Translations)).ToList()
            }).ToList();
        }

        public static AppContentCreateEditViewModel MapToEditViewModel(this AppContentDto dto,IEnumerable<AppLanguageDto> languages)
        {
            return new AppContentCreateEditViewModel
            {
                Id = dto.Id,
                ContentType = dto.ContentType,
                Translations = languages.Select(lang => MapToCreateEditTranslationViewModel(lang, dto.Translations)).ToList()
            };
        }

        public static AppContentCreateEditDto MapToDto(this AppContentCreateEditViewModel viewModel)
        {
            return new AppContentCreateEditDto
            {
                Id = viewModel.Id,
                ContentType = viewModel.ContentType,
                Translations = viewModel.Translations.Select(MapToCreateEditDto).ToList()
            };
        }

        public static void ToEmptyCreateEditViewModel(this AppContentCreateEditViewModel model, IEnumerable<AppLanguageDto> languages)
        {
            model.Translations = languages.Select(lang => new AppContentTranslationCreateEditViewModel
            {
                AppLanguageId = lang.Id,
                LanguageName = lang.Name,
                LanguageIcon = lang.Icon,
                LanguageIsDefault = lang.IsDefault
            }).ToList();
        }

        public static AppContentViewModel ToViewModel(this AppContentDto slide)
        {
            return new AppContentViewModel
            {
                Id = slide.Id,
                ContentType = slide.ContentType,
                Translations = slide.Translations.Select(t => new AppContentTranslationViewModel
                {
                    Id = t.Id,
                    AppLanguageId = t.AppLanguageId,
                    Title = t.Title,
                    Content = t.Content,
                    MetaDescription = t.MetaDescription,
                    ShortDescription = t.ShortDescription

                }).ToList()
            };
        }

        #region Private Helpers

        private static AppContentTranslationViewModel MapToTranslationViewModel(AppLanguageDto lang, IEnumerable<AppContentTranslationDto> translations)
        {
            var t = translations.FirstOrDefault(x => x.AppLanguageId == lang.Id);
            return new AppContentTranslationViewModel
            {
                Id = t?.Id ?? 0,
                ShortDescription = t?.ShortDescription,
                MetaDescription = t?.MetaDescription,
                MetaTitle = t?.MetaTitle,
                Url = t?.Url ?? string.Empty,
                AppLanguageId = lang.Id,
                LanguageName = lang.Name,
                LanguageIcon = lang.Icon,
                Title = t?.Title ?? string.Empty,
                Content = t?.Content ?? string.Empty,
                LanguageIsDefault = lang.IsDefault
            };
        }

        private static AppContentTranslationCreateEditViewModel MapToCreateEditTranslationViewModel(AppLanguageDto lang, IEnumerable<AppContentTranslationDto> translations)
        {
            var t = translations.FirstOrDefault(x => x.AppLanguageId == lang.Id);
            return new AppContentTranslationCreateEditViewModel
            {
                Id = t?.Id ?? 0,
                AppLanguageId = lang.Id,
                LanguageName = lang.Name,
                LanguageIcon = lang.Icon,
                Title = t?.Title ?? string.Empty,
                Content = t?.Content ?? string.Empty,
                MetaDescription = t.MetaDescription,
                ShortDescription = t.ShortDescription,
                MetaTitle = t.MetaTitle,
                Url =t.Url,
                LanguageIsDefault = lang.IsDefault
            };
        }

        private static AppContentTranslationCreateEditDto MapToCreateEditDto(AppContentTranslationCreateEditViewModel vm)
        {
            return new AppContentTranslationCreateEditDto
            {
                Id = vm.Id,
                AppLanguageId = vm.AppLanguageId,
                Title = vm.Title,
                Content = vm.Content,
                MetaDescription = vm.MetaDescription,
                ShortDescription=vm.ShortDescription,
                MetaTitle = vm.MetaTitle,
                Url = vm.Url,
            };
        }

        #endregion
    }


}
