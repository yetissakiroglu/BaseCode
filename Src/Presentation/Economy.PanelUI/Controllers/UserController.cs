using Economy.Base.Application.Dtos.BaseModels;
using Economy.Panel.Application.Dtos.UserDtos;
using Economy.Panel.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Economy.Panel.UI.Controllers
{
    public class UserController : BaseController
    {
        private readonly IPanelAppUserService _panelAppUserService;

        public UserController(IPanelAppUserService panelAppUserService)
        {
            _panelAppUserService = panelAppUserService;
        }

        public IActionResult UserList()
        {
            var result = new List<UserDto>();
            return View(result);
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

    }
}
