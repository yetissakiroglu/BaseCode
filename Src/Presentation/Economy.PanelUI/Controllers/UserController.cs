using Economy.Base.Application.Dtos.BaseModels;
using Economy.Panel.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Economy.Panel.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : BaseController
    {
        private readonly IPanelAppUserService _panelAppUserService;

        public UserController(IPanelAppUserService panelAppUserService)
        {
            _panelAppUserService = panelAppUserService;
        }

        public IActionResult UserList()
        {
            var result = _panelAppUserService.UserList(false);
            return View(result.Data);
        }
        public IActionResult CreateUser()
        {
            var result = new AppUserCreateDto();
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(AppUserCreateDto model)
        {
            var result = await _panelAppUserService.CreateUser(model);
            AddMessage(result);
            return RedirectToAction(nameof(UserList));
        }

        [HttpGet]
        public IActionResult EditUser(int id)
        {
            var result = _panelAppUserService.GetUser(id, false);
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
        public IActionResult DetailsUser(int id)
        {
            var result = _panelAppUserService.GetUser(id, false);
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
