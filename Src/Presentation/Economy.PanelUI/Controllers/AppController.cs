using Economy.Application.Dtos.AppDtos;
using Economy.Core.Interfaces.Economy.Panel.Persistence.Services;
using Economy.Panel.Application.Dtos.AppDtos;
using Economy.Panel.Application.Interfaces;
using Economy.Panel.UI.Models.AppViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Economy.Panel.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AppController : BaseController
    {
        private readonly IPanelAppService _panelAppService;

        public AppController(IPanelAppService panelAppService)
        {
            _panelAppService = panelAppService;
        }

        public IActionResult AppList()
        {
            var result = _panelAppService.Apps(false);
            return View(result.Data);
        }

        [HttpGet]
        public IActionResult CreateApp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateApp(AppCreateViewModel viewModel)
        {
            var appCreate = new AppCreateEditDto()
            {
                DatabaseName = viewModel.DatabaseName,
                Domain = viewModel.Domain,
                HotelName = viewModel.HotelName,
                IsPassword = viewModel.IsPassword,
                Password = viewModel.Password,
                ServerName = viewModel.ServerName,
                UserName = viewModel.UserName
            };


            var result = await _panelAppService.CreateApp(appCreate);
            AddValidationErrorsToModelState(result.ValidationErrors);
            AddMessage(result);
            if (!result.IsSuccess)
            {
                return View(viewModel);
            }

            return RedirectToAction(nameof(AppList));
        }

        [HttpGet]
        public IActionResult EditApp(int Id)
        {
            var result = _panelAppService.GetApp(Id, false);   
            var editDto = new AppEditDto
            {
                Id = result.Data.Id,
                HotelName = result.Data.HotelName,
                ServerName = result.Data.ServerName,
                DatabaseName = result.Data.DatabaseName,
                UserName = result.Data.UserName,
                IsPassword = result.Data.IsPassword,
                Password = result.Data.Password,
                Domain = result.Data.Domain
            };
            return View(editDto);
        }

        [HttpPost]
        public async Task<IActionResult> EditApp(AppEditDto modelDto)
        {
              var editModel = await _panelAppService.EditApp(modelDto);
            AddMessage(editModel);
            return RedirectToAction(nameof(AppList));
        }


        [HttpGet]
        public IActionResult DetailsApp(int Id)
        {
            var result = _panelAppService.GetApp(Id, false);
            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteApp(int Id)
        {
            var result = _panelAppService.DeleteApp(Id);
            if(result.IsSuccess)
            {
                result.Message.RedirectUrl = "/App/"+ nameof(AppList);
            }
            string json = JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });

            return Json(result);
        }


    }
}
