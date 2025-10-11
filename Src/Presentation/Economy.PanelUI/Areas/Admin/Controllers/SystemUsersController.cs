using Economy.Application.AdminUI.Dtos.AppSuperAdminUserDtos;
using Economy.Application.AdminUI.Interfaces;
using Economy.Panel.UI.Controllers;
using Economy.Panel.UI.Models.SuperAdminViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Economy.Panel.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class SystemUsersController : BaseController
    {
        private readonly IPanelSuperAdminService _panelSuperAdminService;
        public SystemUsersController(IPanelSuperAdminService panelSuperAdminService)
        {
            _panelSuperAdminService = panelSuperAdminService;
        }

        #region Kullanıcı 
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
                RolesName = s.RolesName
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
                JobTitle = viewModel.JobTitle,
                LockoutEnabled = viewModel.LockoutEnabled,
                LockoutEnd = viewModel.LockoutEnd,
                SelectedRole = viewModel.SelectedRole
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
                JobTitle = result.Data.JobTitle,
                LockoutEnabled = result.Data.LockoutEnabled,
                LockoutEnd = result.Data.LockoutEnd,
                RoleOptions = roleResult.HasData
                    ? roleResult.Data!.Select(r => new SelectListItem { Text = r.Name, Value = r.Name }).ToList()
                    : new List<SelectListItem>(),
                SelectedRole = result.Data.RolesName,
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
                JobTitle = viewModel.JobTitle,
                LockoutEnabled = viewModel.LockoutEnabled,
                SelectedRole = viewModel.SelectedRole,
                LockoutEnd = viewModel.LockoutEnd
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

            var changePasswordDto = new AppSuperAdminChangePasswordDto
            {
                UserId = model.UserId,
                NewPassword = model.NewPassword,
                ConfirmPassword = model.ConfirmPassword,
            };

            var result = await _panelSuperAdminService.ChangePasswordAsync(changePasswordDto);

            if (result.IsSuccess)
            {
                AddMessage(result);
                return RedirectToAction("ChangePassword");
            }

            AddValidationErrorsToModelState(result.ValidationErrors);
            return View(model);
        }
        #endregion

        #region Role 
        [HttpGet]
        public async Task<IActionResult> Roles()
        {
            var roles = await _panelSuperAdminService.GetRolesAsync();
            if (!roles.HasData)
            {
                AddMessage(roles);
                return View(roles);
            }

            var viewModels = roles?.Data?.Select(s => new RoleViewModel
            {
                RoleId = s.RoleId,
                Name = s.Name,

            }).ToList();

            return View(viewModels);
        }

        //// /SuperAdmin/CreateRole (GET)
        //public IActionResult CreateRole() => View(new CreateRoleViewModel());

        //// /SuperAdmin/CreateRole (POST)
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        //{
        //    if (!ModelState.IsValid) return View(model);

        //    var exists = await _roleManager.RoleExistsAsync(model.Name.Trim());
        //    if (exists)
        //    {
        //        ModelState.AddModelError("", "Bu isimde bir rol zaten mevcut.");
        //        return View(model);
        //    }
        //    var role = new AppRole
        //    {
        //        Name = model.Name.Trim()
        //    };
        //    var result = await _roleManager.CreateAsync(role);
        //    if (result.Succeeded)
        //    {
        //        TempData["Success"] = "Rol başarıyla oluşturuldu.";
        //        return RedirectToAction(nameof(Roles));
        //    }

        //    foreach (var e in result.Errors) ModelState.AddModelError("", e.Description);
        //    return View(model);
        //}

        //// /SuperAdmin/EditRole/{id} (GET)
        //[HttpGet]
        //public async Task<IActionResult> EditRole(int id)
        //{
        //    var role = await _roleManager.FindByIdAsync(id.ToString());
        //    if (role == null)
        //    {
        //        TempData["Error"] = "Rol bulunamadı.";
        //        return RedirectToAction(nameof(Roles));
        //    }
        //    return View(new EditRoleViewModel { Id = role.Id, Name = role.Name! });
        //}

        //// /SuperAdmin/EditRole/{id} (POST)
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EditRole(int id, EditRoleViewModel model)
        //{
        //    if (!ModelState.IsValid) return View(model);

        //    var role = await _roleManager.FindByIdAsync(id.ToString());
        //    if (role == null)
        //    {
        //        TempData["Error"] = "Rol bulunamadı.";
        //        return RedirectToAction(nameof(Roles));
        //    }

        //    role.Name = model.Name.Trim();
        //    var result = await _roleManager.UpdateAsync(role);
        //    if (result.Succeeded)
        //    {
        //        TempData["Success"] = "Rol güncellendi.";
        //        return RedirectToAction(nameof(Roles));
        //    }

        //    foreach (var e in result.Errors) ModelState.AddModelError("", e.Description);
        //    return View(model);
        //}

        //// /SuperAdmin/DeleteRole (POST)
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteRole(string id)
        //{
        //    var role = await _roleManager.FindByIdAsync(id);
        //    if (role == null)
        //    {
        //        TempData["Error"] = "Rol bulunamadı.";
        //        return RedirectToAction(nameof(Roles));
        //    }

        //    var result = await _roleManager.DeleteAsync(role);
        //    TempData[result.Succeeded ? "Success" : "Error"] =
        //        result.Succeeded ? "Rol silindi." : string.Join(" ", result.Errors.Select(e => e.Description));

        //    return RedirectToAction(nameof(Roles));
        //}
        #endregion
    }
}
