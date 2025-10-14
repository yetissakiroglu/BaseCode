using Economy.Application.TenantUI.Dtos.AppPageDtos;
using Economy.Application.TenantUI.Dtos.AppPageMediaDtos;
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
    public sealed class PagesEskiController : BaseController
    {
        private readonly IPanelAppPageService _panelAppPageService;
        private readonly IPanelAppPageMediaService _panelAppPageMediaService;
        public PagesEskiController(IUnitOfWork uow, IPanelAppPageService panelAppPageService, IPanelAppPageMediaService panelAppPageMediaService)
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
            if (!ModelState.IsValid)
            {
                await _panelAppPageService.EnsureLanguageTabsAsync(vm, ct);
                ViewBag.Parents = (await _panelAppPageService.GetParentOptionsAsync(ct)).Data;
                return View(vm);
            }

            var result = await _panelAppPageService.Create(vm, ct);
            AddMessage(result);
            if (!result.IsSuccess)
            {
                return RedirectToAction(nameof(Create), vm);
            }

            var galeri = vm.Galleries.FirstOrDefault(x => x.Key == "GenelImages");
            foreach (var item in galeri.Items)
            {
                if (item.Id == 0)
                {
                    var mediaResult = await _panelAppPageMediaService.Create(new PageMediaEditDto
                    {
                        IsCover = galeri.CoverUrl == item.MediaUrl ? false : true,
                        ContentItemId = result.Data.Id,
                        MediaUrl = item.MediaUrl,
                    }, ct);
                }
            }




            return RedirectToAction(nameof(Edit), new { id = result.Data.Id });
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
            if (!ModelState.IsValid)
            {
                await _panelAppPageService.EnsureLanguageTabsAsync(vm, ct);
                ViewBag.Parents = (await _panelAppPageService.GetParentOptionsAsync(ct, excludeId: id)).Data;
                return View(vm);
            }

            var resultEdit = await _panelAppPageService.Edit(id, vm, ct);


            AddMessage(resultEdit);

            var galeri = vm.Galleries.FirstOrDefault(x => x.Key == "GenelImages");
            foreach (var item in galeri.Items)
            {
                if (item.Id == 0)
                {
                    var mediaResult = await _panelAppPageMediaService.Create(new PageMediaEditDto
                    {
                        IsCover = galeri.CoverUrl == item.MediaUrl ? false : true,
                        ContentItemId = resultEdit.Data.Id,
                        MediaUrl = item.MediaUrl,
                    }, ct);
                }
            }






            return RedirectToAction(nameof(Edit), new { id });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var resultDelete = await _panelAppPageService.Delete(id, ct);
            AddMessage(resultDelete);
            return RedirectToAction(nameof(Index));
        }




        public IActionResult CreateTest()
        {
            var vm = new PageCreateVm
            {
                Singles =
            {
                new ImageFieldVm { Key="KapakImage", Label="Kapak Görseli" },
                new ImageFieldVm { Key="OgKapakImage", Label="OG Kapak Görseli" }
            },
                Galleries =
            {
                new GalleryGroupVm { Key="galeri", Label="Galeri Fotoğrafları" },
                new GalleryGroupVm { Key="oda", Label="Oda Fotoğrafları" }
            }
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTest(PageCreateVm vm)
        {
            if (!ModelState.IsValid) return View(vm);

            // TODO: DB kaydı (vm.Title, vm.Content, vm.ImageUrl (kapak), vm.ImageUrls (galeri))
            TempData["ok"] = "Sayfa oluşturuldu (demo).";
            return RedirectToAction(nameof(Create));
        }

        public class PageCreateVm
        {
            public string Title { get; set; } = string.Empty;
            public string? Content { get; set; }

            public string? ImageUrl { get; set; }       // Kapak
            public string? OgImageUrl { get; set; }       // Kapak

            public List<string> ImageUrls { get; set; } = new(); // Galeri


            public List<ImageFieldVm> Singles { get; set; } = new();
            public List<GalleryGroupVm> Galleries { get; set; } = new();
        }

    }
}
