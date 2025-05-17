using Economy.Panel.Application.Dtos.AppSettingWhatsappLineDtos;
using Economy.Panel.Application.Interfaces;
using Economy.Panel.UI.Models.WhatsappLineViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Controllers
{
    public class WhatsappLineController : BaseController
    {
        private readonly IPanelAppSettingWhatsappLineService _whatsappLineService;

        public WhatsappLineController(IPanelAppSettingWhatsappLineService whatsappLineService)
        {
            _whatsappLineService = whatsappLineService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var response = _whatsappLineService.GetAllAppSettingWhatsappLine(false);

            var resultModel = response.Data.Select(x => new AppSettingWhatsappLineListViewModel
            {
                Id = x.Id,
                CountryCode = x.CountryCode,
                Number = x.Number,
                IsPrimary = x.IsPrimary,
            }).ToList();

            return View(resultModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(AppSettingWhatsappLineCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = _whatsappLineService.CreateAppSettingWhatsappLine(new AppSettingWhatsappLineCreateDto
                {
                    CountryCode = model.CountryCode,
                    Number = model.Number,
                    IsPrimary = model.IsPrimary
                });
                AddMessage(response);
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var response = _whatsappLineService.GetAppSettingWhatsappLine(id, false);
            if (response.Data == null)
            {
                return NotFound();
            }
            var model = new AppSettingWhatsappLineCreateViewModel
            {
                CountryCode = response.Data.CountryCode,
                Number = response.Data.Number,
                IsPrimary = response.Data.IsPrimary
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(AppSettingWhatsappLineCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = _whatsappLineService.EditAppSettingWhatsappLine(new AppSettingWhatsappLineEditDto
                {
                    CountryCode = model.CountryCode,
                    Number = model.Number,
                    IsPrimary = model.IsPrimary
                });
                AddMessage(response);
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var response = _whatsappLineService.DeleteAppSettingWhatsappLine(id);
            AddMessage(response);
            return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult Details(int Id)
        {
            var response = _whatsappLineService.GetAppSettingWhatsappLine(Id, false);
            if (!response.IsSuccess || response.Data == null)
            {
                AddMessage(response); // Hata mesajı göster
                return RedirectToAction("Index");
            }
            var model = new AppSettingWhatsappLineCreateViewModel
            {
                CountryCode = response.Data.CountryCode,
                Number = response.Data.Number,
                IsPrimary = response.Data.IsPrimary
            };
            return View(model);
        }





    }
}
