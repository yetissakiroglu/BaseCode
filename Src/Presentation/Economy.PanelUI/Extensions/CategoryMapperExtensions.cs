using Economy.Panel.Application.Dtos.AppCategoryDtos;
using Economy.Panel.Application.Dtos.AppCategoryDtos.CategoryTranslationDtos;
using Economy.Panel.Application.Dtos.AppLanguageDtos;
using Economy.Panel.UI.Models.CategoryViewModels;
using Economy.Panel.UI.Models.CategoryViewModels.AppCategoryTranslationViewModels;

namespace Economy.Panel.UI.Extensions
{
    /// <summary>
    /// Extension methods for mapping and creating ViewModel and DTO objects related to AppSlide.
    /// </summary>
    public static class CategoryMapperExtensions
    {
        public static List<AppCategoryListViewModel> MapToListViewModel(this IEnumerable<AppCategoryDto> slides,IEnumerable<AppLanguageDto> languages)
        {
            return slides.Select(slide => new AppCategoryListViewModel
            {
                Id = slide.Id,
                Translations = languages.Select(lang => MapToTranslationViewModel(lang, slide.Translations)).ToList()
            }).ToList();
        }

        public static AppCategoryCreateEditViewModel MapToEditViewModel(this AppCategoryDto dto,IEnumerable<AppLanguageDto> languages)
        {
            return new AppCategoryCreateEditViewModel
            {
                Id = dto.Id,
                Translations = languages.Select(lang => MapToCreateEditTranslationViewModel(lang, dto.Translations)).ToList()
            };
        }

        public static AppCategoryCreateEditDto MapToDto(this AppCategoryCreateEditViewModel viewModel)
        {
            return new AppCategoryCreateEditDto
            {
                Id = viewModel.Id,
                Translations = viewModel.Translations.Select(MapToCreateEditDto).ToList()
            };
        }

        //public static AppCategoryCreateEditViewModel ToEmptyCreateEditViewModel(this IEnumerable<AppLanguageDto> languages)
        //{
        //    return new AppCategoryCreateEditViewModel
        //    {
        //        Translations = languages.Select(lang => new AppCategoryTranslationCreateEditViewModel
        //        {
        //            AppLanguageId = lang.Id,
        //            LanguageName = lang.Name,
        //            LanguageIcon = lang.Icon,
        //            LanguageIsDefault = lang.IsDefault
        //        }).ToList()
        //    };
        //}

        public static AppCategoryViewModel ToViewModel(this AppCategoryDto slide)
        {
            return new AppCategoryViewModel
            {
                Id = slide.Id,
                ContentType = slide.ContentType,
                ParentCategoryId = slide.ParentCategoryId,
                SubCategories = slide.SubCategories.Select(ToViewModel).ToList(),
                Translations = slide.Translations.Select(t => new AppCategoryTranslationViewModel
                {
                    Id = t.Id,
                    AppCategoryId = t.AppCategoryId,
                    AppLanguageId = t.AppLanguageId,
                    Title = t.Title,
                    Content = t.Content,
                    MetaDescription = t.MetaDescription,
                    ShortDescription = t.ShortDescription

                }).ToList()
            };
        }

        #region Private Helpers

        private static AppCategoryTranslationViewModel MapToTranslationViewModel(AppLanguageDto lang, IEnumerable<AppCategoryTranslationDto> translations)
        {
            var t = translations.FirstOrDefault(x => x.AppLanguageId == lang.Id);
            return new AppCategoryTranslationViewModel
            {
                Id = t?.Id ?? 0,
                AppCategoryId = t?.AppCategoryId ?? 0,
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

        private static AppCategoryTranslationCreateEditViewModel MapToCreateEditTranslationViewModel(AppLanguageDto lang, IEnumerable<AppCategoryTranslationDto> translations)
        {
            var t = translations.FirstOrDefault(x => x.AppLanguageId == lang.Id);
            return new AppCategoryTranslationCreateEditViewModel
            {
                Id = t?.Id ?? 0,
                AppCategoryId = t?.AppCategoryId ?? 0,
                AppLanguageId = lang.Id,
                LanguageName = lang.Name,
                LanguageIcon = lang.Icon,
                Title = t?.Title ?? string.Empty,
                Content = t?.Content ?? string.Empty,
                LanguageIsDefault = lang.IsDefault
            };
        }

        private static AppCategoryTranslationCreateEditDto MapToCreateEditDto(AppCategoryTranslationCreateEditViewModel vm)
        {
            return new AppCategoryTranslationCreateEditDto
            {
                Id = vm.Id,
                AppCategoryId = vm.AppCategoryId,
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
