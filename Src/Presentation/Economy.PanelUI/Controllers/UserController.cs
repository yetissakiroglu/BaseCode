using Economy.Base.Application.Dtos.BaseModels;
using Economy.Panel.Application.Interfaces;
using Economy.Panel.UI.Models.UserViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;

namespace Economy.Panel.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : BaseController
    {
        private readonly IPanelAppUserService _panelAppUserService;
        private readonly IPanelAppService _panelAppService;
        public UserController(IPanelAppUserService panelAppUserService, IPanelAppService panelAppService)
        {
            _panelAppUserService = panelAppUserService;
            _panelAppService = panelAppService;
        }

        public IActionResult UserList()
        {
            var result = _panelAppUserService.UserList(false);
            if (!result.HasData)
            {
                AddMessage(result);
                return View(result.Data);
            }

            var entity = result.Data.Select(s => new AppUserListDto
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Email = s.Email,
                PhoneNumber = s.PhoneNumber,
                IsDefaultAdmin = s.IsDefaultAdmin,
                TenantId = s.TenantId,
                UserName = s.UserName
            }).ToList();

            return View(result.Data);
        }
        public IActionResult CreateUser()
        {
            var result = new AppUserCreateViewModel();

            var tenants = _panelAppService.Apps(false).Data.Select(x => new UserAppListViewModel
            {
                Id = x.Id,
                Name = x.HotelName
            }).ToList();

            result.Tenants = tenants;

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(AppUserCreateViewModel viewModel)
        {
            var createModel = new AppUserCreateDto
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                TenantId = viewModel.TenantId,
                UserName = viewModel.UserName,
                Email = viewModel.Email,
                Password = viewModel.Password,
                PhoneNumber = viewModel.PhoneNumber,
                EmailConfirmed = viewModel.EmailConfirmed,
                PhoneNumberConfirmed = viewModel.PhoneNumberConfirmed
            };

            var result = await _panelAppUserService.CreateUser(createModel);
            AddValidationErrorsToModelState(result.ValidationErrors);
            AddMessage(result);

            if (!result.IsSuccess)
            {
                var userApps = _panelAppService.Apps(false);

                var tenants = _panelAppService.Apps(false).Data.Select(x => new UserAppListViewModel
                {
                    Id = x.Id,
                    Name = x.HotelName
                }).ToList();

                viewModel.Tenants = tenants;

                return View(viewModel);
            }

            return RedirectToAction("UserList");

        }

        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            var result = await _panelAppUserService.GetUser(id, false);
            var resultModel = new AppUserEditDto
            {
                Id = result.Data.Id,
                FirstName = result.Data.FirstName,
                LastName = result.Data.LastName,
                UserName = result.Data.UserName,
                TenantId= result.Data.TenantId,
                Email = result.Data.Email,
                PhoneNumber = result.Data.PhoneNumber
            };

            return View(resultModel);
        }
        [HttpPost]
        public IActionResult EditUser(AppUserEditDto model)
        {
            var result = _panelAppUserService.EditUser(model);
            AddMessage(result);
            return RedirectToAction(nameof(UserList));
        }

        [HttpGet]
        public async Task<IActionResult> DetailsUser(int id)
        {
            var result =await _panelAppUserService.GetUser(id, false);
            return View(result.Data);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteUser(int Id)
        {
            var result = _panelAppUserService.DeleteUser(Id);
            if (result.IsSuccess)
            {
                result.Message.RedirectUrl = "/User/" + nameof(UserList);
            }
            string json = JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });

            return Json(result);
        }

        


    }
}
