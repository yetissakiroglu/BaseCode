using Economy.Panel.Application.Dtos.AppSettingReservationLinkDtos;
using Economy.Panel.Application.Interfaces;
using Economy.Panel.UI.Controllers;
using Economy.Panel.UI.Models.ReservationLinkViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Areas.Tenant.Controllers
{
    [Area("Tenant")]
    [Authorize]
    public class ReservationLinkController : BaseController
    {
        private readonly IPanelAppSettingReservationLinkService _panelAppSettingReservationLinkService;

        public ReservationLinkController(IPanelAppSettingReservationLinkService panelAppSettingReservationLinkService)
        {
            _panelAppSettingReservationLinkService = panelAppSettingReservationLinkService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var response = _panelAppSettingReservationLinkService.GetAllReservationLink(false);
            var modelList = response.Data.Select(x => new AppSettingReservationLinkListViewModel
            {
                Id = x.Id,
                Url = x.Url
            }).ToList();
            return View(modelList);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AppSettingReservationLinkCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Hatalıysa tekrar form gösterilir
            }
            var createDto = new AppSettingReservationLinkCreateDto
            {
                Url = model.Url,

                // Burada diğer alanları da doldurabilirsiniz
            };
            var result = _panelAppSettingReservationLinkService.CreateReservationLink(createDto);
            AddMessage(result);
            if (!result.IsSuccess)
            {
                return View(model); // Hata varsa tekrar göster
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var response = _panelAppSettingReservationLinkService.GetReservationLink(id, false);
            if (response.Data == null)
            {
                return NotFound();
            }
            var model = new AppSettingReservationLinkEditViewModel
            {
                Id = response.Data.Id,
                Url = response.Data.Url
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AppSettingReservationLinkEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Hatalıysa tekrar form gösterilir
            }
            var editDto = new AppSettingReservationLinkEditDto
            {
                Id = model.Id,
                Url = model.Url
            };
            var result = _panelAppSettingReservationLinkService.EditReservationLink(editDto);
            AddMessage(result);
            if (!result.IsSuccess)
            {
                return View(model); // Hata varsa tekrar göster
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var response = _panelAppSettingReservationLinkService.DeleteReservationLink(id);
            AddMessage(response);
            if (!response.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var response = _panelAppSettingReservationLinkService.GetReservationLink(id, false);
            if (response.Data == null)
            {
                return NotFound();
            }
            var model = new AppSettingReservationLinkViewModel
            {
                Id = response.Data.Id,
                Url = response.Data.Url
            };
            return View(model);
        }



    }
}
