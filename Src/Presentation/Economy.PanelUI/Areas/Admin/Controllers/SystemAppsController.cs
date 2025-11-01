using Economy.Application.AdminUI.Dtos.AppDtos;
using Economy.Application.AdminUI.Interfaces;
using Economy.Panel.UI.Areas.Admin.Models.AppViewModels;
using Economy.Panel.UI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography;
using System.Text.Json;

namespace Economy.Panel.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class SystemAppsController : BaseController
    {
        private readonly IPanelAppService _panelAppService;
        private readonly IPanelAppManagerService _panelAppManagerService;
        private readonly IConnectionTesterService _connectionTesterService;
        private readonly IPanelSuperAdminService _panelSuperAdminService;

        public SystemAppsController(IPanelAppService panelAppService, IPanelAppManagerService panelAppManagerService, IConnectionTesterService connectionTesterService, IPanelSuperAdminService panelSuperAdminService)
        {
            _panelAppService = panelAppService;
            _panelAppManagerService = panelAppManagerService;
            _connectionTesterService = connectionTesterService;
            _panelSuperAdminService = panelSuperAdminService;
        }

        public IActionResult AppList()
        {
            var result = _panelAppService.Apps(false);
            return View(result.Data);
        }

        [HttpGet]
        public IActionResult CreateApp()
        {
            var newApp = new AppCreateViewModel
            {
                ApiKey = GenerateApiKey() // otomatik üret
            };
            return View(newApp);
        }
        private string GenerateApiKey()
        {
            var key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            return key.Replace("+", "").Replace("/", "").Replace("=", "");
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
                UserName = viewModel.UserName,
                AccessMode = viewModel.AccessMode,
                Theme = viewModel.Theme
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
            var editDto = new AppEditViewModel
            {
                Id = result.Data.Id,
                HotelName = result.Data.HotelName,
                ServerName = result.Data.ServerName,
                DatabaseName = result.Data.DatabaseName,
                UserName = result.Data.UserName,
                IsPassword = result.Data.IsPassword,
                Password = result.Data.Password,
                Domain = result.Data.Domain,
                AccessMode = result.Data.AccessMode,
                Theme = result.Data.Theme,
                ApiKey = string.IsNullOrWhiteSpace(result.Data.ApiKey) ? GenerateApiKey() : result.Data.ApiKey
            };
            return View(editDto);
        }

        [HttpPost]
        public async Task<IActionResult> EditApp(AppEditViewModel viewModel)
        {
            var modelDto = new AppCreateEditDto()
            {
                Id = viewModel.Id,
                ApiKey = viewModel.ApiKey,
                DatabaseName = viewModel.DatabaseName,
                Domain = viewModel.Domain,
                HotelName = viewModel.HotelName,
                IsPassword = viewModel.IsPassword,
                Password = viewModel.Password,
                ServerName = viewModel.ServerName,
                UserName = viewModel.UserName,
                AccessMode = viewModel.AccessMode,
                Theme = viewModel.Theme
            };
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
            if (result.IsSuccess)
            {
                result.RedirectUrl = "/App/" + nameof(AppList);
            }
            return Json(result);
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var result = _panelAppService.DeleteApp(Id);
            AddMessage(result);
            return RedirectToAction("AppList");
        }

        [HttpPost]
        public async Task<IActionResult> TestConnection(int Id)
        {
            var result = await _connectionTesterService.TestConnectionAsync(Id);
            //AddMessage(result);
            if (result.IsSuccess)
            {
                result.RedirectUrl = "/App/" + nameof(AppList);
            }
            string json = JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });

            return Json(result);
        }




        #region App Yönetici Atama
        [HttpGet]
        public IActionResult SelectForManagerAssign()
        {
            var apps = _panelAppService.Apps(false).Data
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.HotelName,
                    //Selected = a.Id == selectedAppId   // seçili app işaretlensin
                }).ToList();

            ViewBag.AppList = apps;
            return View();
        }

        [HttpPost]
        public IActionResult SelectForManagerAssign(int selectedAppId)
        {
            return RedirectToAction("AssignManagers", new { id = selectedAppId });
        }

        public IActionResult SelectForManagerAssignNew(int selectedAppId)
        {
            return RedirectToAction("AssignManagers", new { id = selectedAppId });
        }

        [HttpGet]
        public async Task<IActionResult> AssignManagers(int id)
        {
            var app = _panelAppService.GetAppById(id).Result;
            if (app == null) return NotFound();

            var allManagers = (await _panelSuperAdminService.GetUserListAsync()).Data
                .Select(u => new ManagerItem
                {
                    Id = u.Id,
                    FullName = u.FirstName + " " + u.LastName
                }).ToList();

            var selectedManagerIds = await _panelAppManagerService.GetManagerIdsByAppIdAsync(id);

            var model = new AssignManagersViewModel
            {
                AppId = app.Data.Id,
                AppName = app.Data.HotelName,
                AllManagers = allManagers,
                SelectedManagerIds = selectedManagerIds
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AssignManagers(AssignManagersViewModel model)
        {
            if (model.SelectedManagerIds == null)
                model.SelectedManagerIds = new List<int>();

            var updateData = await _panelAppManagerService.UpdateManagersForAppAsync(model.AppId, model.SelectedManagerIds);
            AddMessage(updateData);
            return RedirectToAction("AssignManagers", new { id = model.AppId });
        }
        #endregion


    }
}
