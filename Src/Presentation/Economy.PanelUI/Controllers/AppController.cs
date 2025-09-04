using Economy.Application.Dtos.AppDtos;
using Economy.Application.Interfaces;
using Economy.Panel.Application.Dtos.AppDtos;
using Economy.Panel.Application.Interfaces;
using Economy.Panel.UI.Models.AppViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;

namespace Economy.Panel.UI.Controllers
{
    [Authorize]
    public class AppController : BaseController
    {
        private readonly IPanelAppService _panelAppService;
        private readonly IPanelAppUserService _panelAppUserService;
        private readonly IPanelAppManagerService _panelAppManagerService;
        private readonly IConnectionTesterService _connectionTesterService;
        public AppController(IPanelAppService panelAppService, IPanelAppUserService panelAppUserService, IPanelAppManagerService panelAppManagerService, IConnectionTesterService connectionTesterService)
        {
            _panelAppService = panelAppService;
            _panelAppUserService = panelAppUserService;
            _panelAppManagerService = panelAppManagerService;
            _connectionTesterService = connectionTesterService;
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
            var editDto = new AppEditDto
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
            if (result.IsSuccess)
            {
                result.Message.RedirectUrl = "/App/" + nameof(AppList);
            }
            string json = JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });

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

            var allManagers = _panelAppUserService.GetAllManagers().Result.Data
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
