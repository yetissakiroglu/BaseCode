using Economy.Application.Dtos.AppSuperAdminUserDtos;
using Economy.Application.Interfaces;
using Economy.Domain.Entites.Identities;
using Economy.Panel.UI.Models.ProfileViewModels;
using Economy.Panel.UI.Models.SuperAdminViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Economy.Panel.UI.Controllers
{
    [Authorize]
    public class SuperAdminController : BaseController
    {
        private readonly IPanelSuperAdminService _panelSuperAdminService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public SuperAdminController(IPanelSuperAdminService panelSuperAdminService, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _panelSuperAdminService = panelSuperAdminService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> List()
        {
            var superAdmins = await _panelSuperAdminService.GetUserListAsync();
            if (!superAdmins.HasData)
            {
                AddMessage(superAdmins);
                return View(new List<SuperAdminViewModel>());
            }

            var listSuparAdminViewModels = superAdmins?.Data?.Select(s => new SuperAdminViewModel
            {
                UserId = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Email = s.Email,
                PhoneNumber = s.PhoneNumber,
                IsDefaultAdmin = s.IsDefaultAdmin,
                UserName = s.UserName,
                IsDeleted = s.IsDeleted,
                EmailConfirmed = s.EmailConfirmed,
                JobTitle = s.JobTitle,
                PhoneNumberConfirmed = s.PhoneNumberConfirmed,
                PhotoUrl = s.PhotoUrl
            }).ToList();

            return View(listSuparAdminViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var roleResult = await _panelSuperAdminService.GetRolesAsync();

            var model = new SuperAdminCreateViewModel
            {
                RoleOptions = roleResult.HasData
                    ? roleResult.Data!.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList()
                    : new List<SelectListItem>()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SuperAdminCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var roleResult = await _panelSuperAdminService.GetRolesAsync();
                viewModel.RoleOptions = roleResult.HasData
                    ? roleResult.Data!.Select(r => new SelectListItem { Text = r.Name, Value = r.Name }).ToList()
                    : new List<SelectListItem>();

                return View(viewModel);
            }

            var userDto = new AppSuperAdminUserCreateDto
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                UserName = viewModel.UserName,
                Email = viewModel.Email,
                Password = viewModel.Password,
                PhoneNumber = viewModel.PhoneNumber,
                EmailConfirmed = viewModel.EmailConfirmed,
                PhoneNumberConfirmed = viewModel.PhoneNumberConfirmed,
                IsDefaultAdmin = viewModel.IsDefaultAdmin,
                PhotoUrl = viewModel.PhotoUrl,
                JobTitle = viewModel.JobTitle,
                TwoFactorEnabled = viewModel.TwoFactorEnabled,
                LockoutEnabled = viewModel.LockoutEnabled,
                SelectedRoles = viewModel.SelectedRoles
            };

            var result = await _panelSuperAdminService.CreateUserAsync(userDto);
            AddValidationErrorsToModelState(result.ValidationErrors);
            AddMessage(result);

            if (!result.IsSuccess)
            {
                var roleResult = await _panelSuperAdminService.GetRolesAsync();
                viewModel.RoleOptions = roleResult.HasData
                    ? roleResult.Data!.Select(r => new SelectListItem { Text = r.Name, Value = r.Name }).ToList()
                    : new List<SelectListItem>();

                return View(viewModel);
            }

            return RedirectToAction("List");
        }
        
        
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _panelSuperAdminService.GetUserAsync(id);
            if (!result.IsSuccess || result.Data == null)
            {
                AddMessage(result);
                return RedirectToAction("List");
            }

            var roleResult = await _panelSuperAdminService.GetRolesAsync();

            var viewModel = new SuperAdminEditViewModel
            {
                UserId = result.Data.Id,
                FirstName = result.Data.FirstName,
                LastName = result.Data.LastName,
                UserName = result.Data.UserName,
                Email = result.Data.Email,
                PhoneNumber = result.Data.PhoneNumber,
                EmailConfirmed = result.Data.EmailConfirmed,
                PhoneNumberConfirmed = result.Data.PhoneNumberConfirmed,
                IsDefaultAdmin = result.Data.IsDefaultAdmin,
                PhotoUrl = result.Data.PhotoUrl,
                JobTitle = result.Data.JobTitle,
                TwoFactorEnabled = result.Data.TwoFactorEnabled,
                LockoutEnabled = result.Data.LockoutEnabled,
                SelectedRoles = result.Data.RolesName,
                RoleOptions = roleResult.HasData
                    ? roleResult.Data!.Select(r => new SelectListItem
                    {
                        Value = r.Name,
                        Text = r.Name,
                        Selected = r.Name == result.Data.RoleName
                    }).ToList()
                    : new List<SelectListItem>()
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(SuperAdminEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var roleResult = await _panelSuperAdminService.GetRolesAsync();
                viewModel.RoleOptions = roleResult.HasData
                    ? roleResult.Data!.Select(r => new SelectListItem { Text = r.Name, Value = r.Name }).ToList()
                    : new List<SelectListItem>();

                return View(viewModel);
            }

            var userDto = new AppSuperAdminUserEditDto
            {
                UserId = viewModel.UserId,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                UserName = viewModel.UserName,
                Email = viewModel.Email,
                PhoneNumber = viewModel.PhoneNumber,
                EmailConfirmed = viewModel.EmailConfirmed,
                PhoneNumberConfirmed = viewModel.PhoneNumberConfirmed,
                IsDefaultAdmin = viewModel.IsDefaultAdmin,
                PhotoUrl = viewModel.PhotoUrl,
                JobTitle = viewModel.JobTitle,
                TwoFactorEnabled = viewModel.TwoFactorEnabled,
                LockoutEnabled = viewModel.LockoutEnabled,
                SelectedRoles = viewModel.SelectedRoles
            };

            var result = await _panelSuperAdminService.UpdateUserAsync(userDto);
            AddValidationErrorsToModelState(result.ValidationErrors);
            AddMessage(result);

            if (!result.IsSuccess)
            {
                var roleResult = await _panelSuperAdminService.GetRolesAsync();
                viewModel.RoleOptions = roleResult.HasData
                    ? roleResult.Data!.Select(r => new SelectListItem { Text = r.Name, Value = r.Name }).ToList()
                    : new List<SelectListItem>();

                return View(viewModel);
            }

            return RedirectToAction("List");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _panelSuperAdminService.DeleteUserAsync(id);
            AddMessage(result);
            return RedirectToAction("List");
        }


        [HttpGet]
        public IActionResult ChangePassword(int id)
        {
            var model = new SuperAdminChangePasswordViewModel
            {
                UserId = id
            };  

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(SuperAdminChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByIdAsync(model.UserId.ToString());
            if (user == null)
            {
                TempData["SuccessMessage"] = "Kullanıcı Bulunamadı.";
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirildi.";
                return RedirectToAction("ChangePassword");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

    }
}
