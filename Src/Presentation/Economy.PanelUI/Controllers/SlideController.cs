using Economy.Panel.Application.Interfaces;
using Economy.Panel.UI.Extensions;
using Economy.Panel.UI.Models.SlideViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Controllers
{
    public class SlideController : BaseController
    {
        private readonly IPanelAppSlideService _panelAppSlideService;
        private readonly IPanelAppLanguageService _panelAppLanguageService;

        public SlideController(IPanelAppSlideService panelAppSlideService, IPanelAppLanguageService panelAppLanguageService)
        {
            _panelAppSlideService = panelAppSlideService;
            _panelAppLanguageService = panelAppLanguageService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var allLanguages = _panelAppLanguageService.GetAllLanguage(false, true);
            var result = _panelAppSlideService.GetAllSlide(false);

            if (!result.IsSuccess || result.Data == null || allLanguages.Data == null)
            {
                return View(new List<AppSlideListViewModel>());
            }

            var resultModel = result.Data.Select(slide => new AppSlideListViewModel
            {
                Id = slide.Id,
                Sequence = slide.Sequence,
                ThumbnailBase64 = slide.ThumbnailBase64,
                ThumbnailMobilBase64 = slide.ThumbnailMobilBase64,
                Translations = allLanguages.Data.Select(lang =>
                {
                    var translation = slide.Translations.FirstOrDefault(t => t.AppLanguageId == lang.Id);

                    return new AppSlideLanguageViewModel
                    {
                        Code = lang.Code,
                        Icon = lang.Icon,
                        IsRTL = lang.IsRTL,
                        Name = lang.Name,
                        Id = translation?.Id ?? 0,
                        AppSlideId = translation?.AppSlideId ?? slide.Id,
                        AppLanguageId = lang.Id,
                        Title = translation?.Title ?? string.Empty,
                        Content = translation?.Content,
                        IsExternal = translation?.IsExternal ?? false,
                        ButtonText = translation?.ButtonText,
                        ButtonUrl = translation?.ButtonUrl,
                        ButtonIcon = translation?.ButtonIcon
                    };
                }).ToList()
            }).ToList();

            return View(resultModel);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var allLanguages = _panelAppLanguageService.GetAllLanguage(false, true);
            var result = _panelAppSlideService.GetSlide(Id, false);

            if (!result.IsSuccess || result.Data == null || allLanguages.Data == null)
            {
                return View(new AppSlideEditViewModel());
            }

            var slide = result.Data;

            var translations = allLanguages.Data.Select(lang =>
            {
                var translation = slide.Translations.FirstOrDefault(t => t.AppLanguageId == lang.Id);

                return new AppSlideLanguageEditViewModel
                {
                    Code = lang.Code,
                    Icon = lang.Icon,
                    IsRTL = lang.IsRTL,
                    Name = lang.Name,
                    Id = translation?.Id ?? 0,
                    AppSlideId = slide.Id,
                    AppLanguageId = lang.Id,
                    Title = translation?.Title ?? string.Empty,
                    Content = translation?.Content ?? string.Empty,
                    IsExternal = translation?.IsExternal ?? false,
                    ButtonText = translation?.ButtonText ?? string.Empty,
                    ButtonUrl = translation?.ButtonUrl ?? string.Empty,
                    ButtonIcon = translation?.ButtonIcon ?? string.Empty
                };
            }).ToList();

            var resultModel = new AppSlideEditViewModel
            {
                Id = slide.Id,
                Sequence = slide.Sequence,
                ThumbnailBase64 = slide.ThumbnailBase64,
                ThumbnailMobilBase64 = slide.ThumbnailMobilBase64,
                Translations = translations
            };

            return View(resultModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AppSlideEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = model.ToDto();
            var result = _panelAppSlideService.EditSlide(dto);

            if (!result.IsSuccess)
            {
                return View(model);
            }
            AddMessage(result);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Create()
        {
            var allLanguages = _panelAppLanguageService.GetAllLanguage(false, true);

            var model = new AppSlideCreateViewModel
            {
                Translations = allLanguages.Data.Select(lang => new AppSlideLanguageCreateViewModel
                {
                    AppLanguageId = lang.Id,
                    Code = lang.Code,
                    Name = lang.Name,
                    Icon = lang.Icon,
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AppSlideCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = model.ToDto();
            var result = _panelAppSlideService.CreateSlide(dto);

            if (!result.IsSuccess)
            {
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var result = _panelAppSlideService.DeleteSlide(id);
            return Json(result);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var allLanguages = _panelAppLanguageService.GetAllLanguage(false, true);

            var result = _panelAppSlideService.GetSlide(id, false);

            if (!result.IsSuccess || result.Data == null)
                return NotFound();

            var slide = result.Data;


            var translations = allLanguages.Data.Select(lang =>
            {
                var translation = slide.Translations.FirstOrDefault(t => t.AppLanguageId == lang.Id);

                return new AppSlideLanguageViewModel
                {
                    Code = lang.Code,
                    Icon = lang.Icon,
                    IsRTL = lang.IsRTL,
                    Name = lang.Name,
                    Id = translation?.Id ?? 0,
                    AppSlideId = slide.Id,
                    AppLanguageId = lang.Id,
                    Title = translation?.Title ?? string.Empty,
                    Content = translation?.Content ?? string.Empty,
                    IsExternal = translation?.IsExternal ?? false,
                    ButtonText = translation?.ButtonText ?? string.Empty,
                    ButtonUrl = translation?.ButtonUrl ?? string.Empty,
                    ButtonIcon = translation?.ButtonIcon ?? string.Empty
                };
            }).ToList();

            var resultModel = new AppSlideViewModel
            {
                Id = slide.Id,
                Sequence = slide.Sequence,
                ThumbnailBase64 = slide.ThumbnailBase64,
                ThumbnailMobilBase64 = slide.ThumbnailMobilBase64,
                Translations = translations
            };

  
            return View(resultModel);
        }
    }

}
