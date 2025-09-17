using Economy.Panel.Application.Dtos.AppSettingReservationNumberDtos;
using Economy.Panel.Application.Interfaces;
using Economy.Panel.UI.Controllers;
using Economy.Panel.UI.Models.ReservationNumberViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Areas.Tenant.Controllers
{
    [Area("Tenant")]
    [Authorize]
    public class ReservationNumberController : BaseController
    {
        private readonly IPanelAppSettingReservationNumberService _panelAppSettingReservationNumberService;

        public ReservationNumberController(IPanelAppSettingReservationNumberService panelAppSettingReservationNumberService)
        {
            _panelAppSettingReservationNumberService = panelAppSettingReservationNumberService;
        }

        public IActionResult Index()
        {
            var response = _panelAppSettingReservationNumberService.GetAllReservationNumber(false);
            var modelList = response.Data.Select(x => new AppSettingReservationNumberListViewModel
            {
                Id = x.Id,
                CountryCode = x.CountryCode,
                IsPrimary = x.IsPrimary,
                Number = x.Number
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
        public IActionResult Create(AppSettingReservationNumberCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Hatalıysa tekrar form gösterilir
            }

            var createDto = new AppSettingReservationNumberCreateDto
            {
                CountryCode = model.CountryCode,
                IsPrimary = model.IsPrimary,
                Number = model.Number
                // Burada diğer alanları da doldurabilirsiniz
            };

            var result = _panelAppSettingReservationNumberService.CreateReservationNumber(createDto);
            AddMessage(result);

            if (!result.IsSuccess)
            {
                return View(model); // Hata varsa tekrar göster
            }

            return RedirectToAction("Index"); // Başarılıysa listeye dön
        }



        [HttpGet]
        public IActionResult Edit(int id)
        {
            var result = _panelAppSettingReservationNumberService.GetReservationNumber(id, false);
            if (!result.IsSuccess || result.Data == null)
            {
                AddMessage(result); // Hata mesajı göster
                return RedirectToAction("Index");
            }
            var reservationNumber = result.Data;
            var viewModel = new AppSettingReservationNumberEditViewModel
            {
                Id = reservationNumber.Id,
                CountryCode = reservationNumber.CountryCode,
                IsPrimary = reservationNumber.IsPrimary,
                Number = reservationNumber.Number
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AppSettingReservationNumberEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Hatalıysa tekrar form gösterilir
            }
            var editDto = new AppSettingReservationNumberEditDto
            {
                Id = model.Id,
                CountryCode = model.CountryCode,
                IsPrimary = model.IsPrimary,
                Number = model.Number
                // Burada diğer alanları da doldurabilirsiniz
            };
            var result = _panelAppSettingReservationNumberService.EditReservationNumber(editDto);
            AddMessage(result);
            if (!result.IsSuccess)
            {
                return View(model); // Hata varsa tekrar göster
            }
            return RedirectToAction("Index"); // Başarılıysa listeye dön
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var result = _panelAppSettingReservationNumberService.DeleteReservationNumber(id);
            AddMessage(result);
            if (!result.IsSuccess)
            {
                return RedirectToAction("Index"); // Hata varsa listeye dön
            }
            return RedirectToAction("Index"); // Başarılıysa listeye dön
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var result = _panelAppSettingReservationNumberService.GetReservationNumber(id, false);
            if (!result.IsSuccess || result.Data == null)
            {
                AddMessage(result); // Hata mesajı göster
                return RedirectToAction("Index");
            }
            var reservationNumber = result.Data;
            var viewModel = new AppSettingReservationNumberViewModel
            {
                Id = reservationNumber.Id,
                CountryCode = reservationNumber.CountryCode,
                IsPrimary = reservationNumber.IsPrimary,
                Number = reservationNumber.Number
            };
            return View(viewModel);
        }
    }
}
