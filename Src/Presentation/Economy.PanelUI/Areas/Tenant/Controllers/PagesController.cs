using Economy.Application.TenantUI.Dtos.AppPageDtos;
using Economy.Application.TenantUI.Interfaces;
using Economy.Core.Dtos.Custom;
using Economy.Core.Interfaces;
using Economy.Panel.UI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Areas.Tenant.Controllers
{
    [Area("Tenant")]
    [Authorize]
    public sealed class PagesController : BaseController
    {
        private readonly IPanelAppPageService _panelAppPageService;
        private readonly IPanelAppPageMediaService _panelAppPageMediaService;
        public PagesController(IUnitOfWork uow, IPanelAppPageService panelAppPageService, IPanelAppPageMediaService panelAppPageMediaService)
        {
            _panelAppPageService = panelAppPageService;
            _panelAppPageMediaService = panelAppPageMediaService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var pageModel = await _panelAppPageService.GetPageListAsync(ct);
            return View(pageModel.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            var vm = new PageEditDto();
            vm.Singles = new List<ImageFieldVm>()
            {
                new ImageFieldVm { Key = "KapakImage", Label = "Kapak Görseli" },
                new ImageFieldVm { Key ="OGImage", Label="OG Görseli"}
            };

            vm.Galleries = new List<GalleryGroupVm>()
            {
                new GalleryGroupVm { Key = "GenelImages", Label = "Galeri Fotoğrafları" },
            };

            await _panelAppPageService.FillLanguagesAsync(vm, ct);
            ViewBag.Parents = (await _panelAppPageService.GetParentOptionsAsync(ct)).Data;
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PageEditDto vm, CancellationToken ct)
        {
            vm.CoverImageUrl = vm.Singles?.FirstOrDefault(x => x.Key == "KapakImage")?.Url;
            vm.OgImageUrl = vm.Singles?.FirstOrDefault(x => x.Key == "OGImage")?.Url;

            if (!ModelState.IsValid)
            {
                await _panelAppPageService.EnsureLanguageTabsAsync(vm, ct);
                ViewBag.Parents = (await _panelAppPageService.GetParentOptionsAsync(ct)).Data;
                return View(vm);
            }

            var resultNew = await _panelAppPageService.CreateEdit(vm, ct);
            AddMessage(resultNew);
            if (!resultNew.IsSuccess)
            {
                return RedirectToAction(nameof(Create), vm);
            }


            //var result = await _panelAppPageService.Create(vm, ct);
            //AddMessage(result);
            //if (!result.IsSuccess)
            //{
            //    return RedirectToAction(nameof(Create), vm);
            //}

            //var galeri = vm.Galleries.FirstOrDefault(x => x.Key == "GenelImages");
            //foreach (var item in galeri.Items)
            //{
            //    if (item.Id == 0)
            //    {
            //        var mediaResult = await _panelAppPageMediaService.Create(new PageMediaEditDto
            //        {
            //            IsCover = galeri.CoverUrl == item.MediaUrl ? false : true,
            //            AppPageId = result.Data.Id,
            //            MediaUrl = item.MediaUrl,
            //        }, ct);
            //    }
            //}




            return RedirectToAction(nameof(Edit), new { id = resultNew.Data.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {

            var result = await _panelAppPageService.GetPageAsync(id, ct);
            if (!result.HasData)
            {
                AddMessage(result);
            }

            ViewBag.Parents = (await _panelAppPageService.GetParentOptionsAsync(ct, excludeId: result.Data.Id)).Data;
            return View(result.Data);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PageEditDto vm, CancellationToken ct)
        {
            vm.CoverImageUrl = vm.Singles?.FirstOrDefault(x => x.Key == "KapakImage")?.Url;
            vm.OgImageUrl = vm.Singles?.FirstOrDefault(x => x.Key == "OGImage")?.Url;

            if (!ModelState.IsValid)
            {
                await _panelAppPageService.EnsureLanguageTabsAsync(vm, ct);
                ViewBag.Parents = (await _panelAppPageService.GetParentOptionsAsync(ct, excludeId: id)).Data;
                return View(vm);
            }
            vm.Id = id;
            var resultEdit = await _panelAppPageService.CreateEdit(vm, ct);
            AddMessage(resultEdit);

          

            return RedirectToAction(nameof(Edit), new { id });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var resultDelete = await _panelAppPageService.Delete(id, ct);
            AddMessage(resultDelete);
            return RedirectToAction(nameof(Index));
        }


    }
}
