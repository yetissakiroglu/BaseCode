using Economy.Application.Interfaces;
using Economy.Application.TenantUI.Dtos.AppPageDtos;
using Economy.Application.TenantUI.Interfaces;
using Economy.Core.Dtos.Custom;
using Economy.Core.Enums;
using Economy.Panel.UI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Areas.Tenant.Controllers
{
    [Area("Tenant")]
    [Authorize]
    public class RoomsController : BaseController
    {
        private readonly IPanelAppPageService _panelAppPageService;
        private readonly IPanelAppPageMediaService _panelAppPageMediaService;
        private readonly ISlugService _slugService;
        public RoomsController(IPanelAppPageService panelAppPageService, IPanelAppPageMediaService panelAppPageMediaService, ISlugService slugService)
        {
            _panelAppPageService = panelAppPageService;
            _panelAppPageMediaService = panelAppPageMediaService;
            _slugService = slugService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var pageModel = await _panelAppPageService.GetPageListAsync(ContentItemType.Room, ct);
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
    }
}
