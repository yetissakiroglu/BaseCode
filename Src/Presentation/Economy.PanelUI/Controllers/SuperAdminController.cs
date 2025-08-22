using Economy.Application.Dtos.AppSuperAdminUserDtos;
using Economy.Application.Dtos.LoginLogPageQueryDto;
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
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IPanelLoginLogService _svc;

        public SuperAdminController(IPanelSuperAdminService panelSuperAdminService, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IPanelLoginLogService svc)
        {
            _panelSuperAdminService = panelSuperAdminService;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _svc = svc;
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
                JobTitle = viewModel.JobTitle,
                LockoutEnabled = viewModel.LockoutEnabled,
                LockoutEnd = viewModel.LockoutEnd,
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
                        Selected = r.Name == result.Data.RolesName.Where(role => role == r.Name).FirstOrDefault()
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
        #endregion

        #region Role 
        [HttpGet]
        public IActionResult Roles()
        {
            var roles = _roleManager.Roles
                .OrderBy(r => r.Name)
                .ToList();

            return View(roles);
        }
        // /SuperAdmin/CreateRole (GET)
        public IActionResult CreateRole() => View(new CreateRoleViewModel());

        // /SuperAdmin/CreateRole (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var exists = await _roleManager.RoleExistsAsync(model.Name.Trim());
            if (exists)
            {
                ModelState.AddModelError("", "Bu isimde bir rol zaten mevcut.");
                return View(model);
            }
            var role = new AppRole
            {
                Name = model.Name.Trim()
            };
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                TempData["Success"] = "Rol başarıyla oluşturuldu.";
                return RedirectToAction(nameof(Roles));
            }

            foreach (var e in result.Errors) ModelState.AddModelError("", e.Description);
            return View(model);
        }

        // /SuperAdmin/EditRole/{id} (GET)
        [HttpGet]
        public async Task<IActionResult> EditRole(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                TempData["Error"] = "Rol bulunamadı.";
                return RedirectToAction(nameof(Roles));
            }
            return View(new EditRoleViewModel { Id = role.Id, Name = role.Name! });
        }

        // /SuperAdmin/EditRole/{id} (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(int id, EditRoleViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                TempData["Error"] = "Rol bulunamadı.";
                return RedirectToAction(nameof(Roles));
            }

            role.Name = model.Name.Trim();
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                TempData["Success"] = "Rol güncellendi.";
                return RedirectToAction(nameof(Roles));
            }

            foreach (var e in result.Errors) ModelState.AddModelError("", e.Description);
            return View(model);
        }

        // /SuperAdmin/DeleteRole (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                TempData["Error"] = "Rol bulunamadı.";
                return RedirectToAction(nameof(Roles));
            }

            var result = await _roleManager.DeleteAsync(role);
            TempData[result.Succeeded ? "Success" : "Error"] =
                result.Succeeded ? "Rol silindi." : string.Join(" ", result.Errors.Select(e => e.Description));

            return RedirectToAction(nameof(Roles));
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> LoginLogs([FromQuery] LoginLogPageQuery q)
        {
            var res = await _svc.GetPageAsync(q);
            if (!res.IsSuccess || res.Data == null)
            {
                TempData["Error"] = res.Message ?? "Kayıtlar alınamadı";
                return View(new LoginLogPageViewModel { Q = q ?? new LoginLogPageQuery() });
            }
            ViewData["Title"] = "Login Logları";
            return View(res.Data);
        }

        [HttpGet("LoginLogs/Export")]
        public async Task<IActionResult> Export([FromQuery] LoginLogPageQuery q)
        {
            var res = await _svc.ExportCsvAsync(q);
            if (!res.IsSuccess || res.Data == null)
                return BadRequest(res.Message ?? "Export hatası");
            return File(res.Data, "text/csv", "login-logs.csv");
        }

    }
}
