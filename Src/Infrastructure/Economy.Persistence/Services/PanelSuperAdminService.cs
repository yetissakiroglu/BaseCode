using Economy.Application.Dtos.AppSuperAdminUserDtos;
using Economy.Application.Dtos.AppUserDtos;
using Economy.Application.Interfaces;
using Economy.Base.Application.Dtos.BaseModels;
using Economy.Core.Interfaces;
using Economy.Core.Tools;
using Economy.Core.Tools.Models;
using Economy.Core.Tools.Result;
using Economy.Domain.Entites.Identities;
using Economy.Panel.Application.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.ObjectModel;
using System.Net;

namespace Economy.Persistence.Services
{
    public class PanelSuperAdminService : IPanelSuperAdminService
    {
        private readonly IEntityRepository<AppUser, int> _superAdminRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        private readonly IValidator<AppSuperAdminUserCreateDto> _validatorCreate;
        private readonly IValidator<AppSuperAdminUserEditDto> _validatorEdit;
        public PanelSuperAdminService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IValidator<AppSuperAdminUserCreateDto> validatorCreate, IValidator<AppSuperAdminUserEditDto> validatorEdit, SignInManager<AppUser> signInManager)
        {
            _unitOfWork = unitOfWork;
            _superAdminRepository = unitOfWork.DefaultEntityRepository<AppUser>();
            _userManager = userManager;
            _roleManager = roleManager;
            _validatorCreate = validatorCreate;
            _validatorEdit = validatorEdit;
            _signInManager = signInManager;
        }
        public async Task<ServiceResult<List<AppSuperAdminUserDto>>> GetUserListAsync()
        {
            // Önce kullanıcıları çekiyoruz
            var users = _superAdminRepository
                .WhereForRead(x => !x.IsDeleted);

            var result = new List<AppSuperAdminUserDto>();

            foreach (var user in users)
            {
                // Rollerini çekiyoruz
                var roles = await _userManager.GetRolesAsync(user);

                // DTO’ya mapliyoruz
                var dto = new AppSuperAdminUserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email,
                    IsDeleted = user.IsDeleted,
                    IsDefaultAdmin = user.IsDefaultAdmin,
                    JobTitle = user.JobTitle,
                    PhoneNumber = user.PhoneNumber,
                    TwoFactorEnabled = user.TwoFactorEnabled,
                    AccessFailedCount = user.AccessFailedCount,
                    ConcurrencyStamp = user.ConcurrencyStamp,
                    EmailConfirmed = user.EmailConfirmed,
                    LockoutEnabled = user.LockoutEnabled,
                    LockoutEnd = user.LockoutEnd,
                    NormalizedEmail = user.NormalizedEmail,
                    NormalizedUserName = user.NormalizedUserName,
                    PasswordHash = user.PasswordHash,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                    SecurityStamp = user.SecurityStamp,
                    RolesName = string.Join(", ", roles) // Virgülle ayırıyoruz
                };

                result.Add(dto);
            }

            return ServiceResult<List<AppSuperAdminUserDto>>.Success(result);
        }
        public async Task<ServiceResult<AppSuperAdminUserDto>> GetUserAsync(int userId)
        {
            var entity = _superAdminRepository.GetForRead(x => x.Id == userId && !x.IsDeleted);
            if (entity == null)
            {
                return ServiceResult<AppSuperAdminUserDto>.Failure("Kullanıcı bulunamadı.");
            }

            // 🔽 Rolü çek
            var roles = await _userManager.GetRolesAsync(entity);

            var dto = new AppSuperAdminUserDto
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                UserName = entity.UserName,
                Email = entity.Email,
                IsDeleted = entity.IsDeleted,
                IsDefaultAdmin = entity.IsDefaultAdmin,
                JobTitle = entity.JobTitle,
                PhoneNumber = entity.PhoneNumber,
                TwoFactorEnabled = entity.TwoFactorEnabled,
                AccessFailedCount = entity.AccessFailedCount,
                ConcurrencyStamp = entity.ConcurrencyStamp,
                EmailConfirmed = entity.EmailConfirmed,
                LockoutEnabled = entity.LockoutEnabled,
                LockoutEnd = entity.LockoutEnd,
                NormalizedEmail = entity.NormalizedEmail,
                NormalizedUserName = entity.NormalizedUserName,
                PasswordHash = entity.PasswordHash,
                PhoneNumberConfirmed = entity.PhoneNumberConfirmed,
                SecurityStamp = entity.SecurityStamp,
                RolesName = roles.Count > 0 ? string.Join(", ", roles) : "Rol Atanmamış"
            };

            return ServiceResult<AppSuperAdminUserDto>.Success(dto);
        }












        public async Task<ServiceResult<List<AppRoleDto>>> GetRolesAsync()
        {
            var roles = await Task.FromResult(
                _roleManager.Roles
                    .Select(role => new AppRoleDto
                    {
                        RoleId = role.Id,
                        Name = role.Name
                    }).ToList()
            );

            if (roles.Any())
                return ServiceResult<List<AppRoleDto>>.Success(roles, "Roller başarıyla alındı.");
            else
                return ServiceResult<List<AppRoleDto>>.Empty("Kayıtlı rol bulunamadı.");
        }


        public async Task<ServiceResult<AppSuperAdminUserDto>> CreateUserAsync(AppSuperAdminUserCreateDto userDto)
        {
            if (userDto == null)
            {
                return ServiceResult<AppSuperAdminUserDto>.Failure(
                    message: "Geçersiz veri gönderildi.",
                    statusCode: (int)HttpStatusCode.BadRequest);
            }

            if (userDto.IsDefaultAdmin)
            {
                return ServiceResult<AppSuperAdminUserDto>.Failure(
                   message: "Default Admin Kullanıcısı oluşturulamaz.",
                   statusCode: (int)HttpStatusCode.NotFound);
            }

            var validationResult = _validatorCreate.Validate(userDto);
            if (!validationResult.IsValid)
            {
                return ServiceResult<AppSuperAdminUserDto>.Failure(
                    message: "Geçersiz giriş verisi.",
                    statusCode: (int)HttpStatusCode.BadRequest,
                    validationErrors: validationResult.ToValidationDictionary());
            }

            var user = new AppUser
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                UserName = userDto.UserName,
                Email = userDto.Email,
                PhoneNumber = userDto.PhoneNumber,
                EmailConfirmed = userDto.EmailConfirmed,
                PhoneNumberConfirmed = userDto.PhoneNumberConfirmed,
                IsDefaultAdmin = userDto.IsDefaultAdmin,
                JobTitle = userDto.JobTitle,
                LockoutEnabled = userDto.LockoutEnabled,
                LockoutEnd = userDto.LockoutEnd
            };

            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (!result.Succeeded)
            {
                return ServiceResult<AppSuperAdminUserDto>.Failure(
                    message: "Kullanıcı oluşturulamadı.",
                    statusCode: (int)HttpStatusCode.BadRequest,
                    validationErrors: result.Errors.ToValidationDictionary());
            }


            var notFoundRoles = new List<string>();

            var trimmedRole = userDto.SelectedRole.Trim();
            if (!string.IsNullOrWhiteSpace(trimmedRole))
            {
                if (await _roleManager.RoleExistsAsync(trimmedRole))
                {
                    await _userManager.AddToRoleAsync(user, trimmedRole);
                }
                else
                {
                    notFoundRoles.Add(trimmedRole);
                }
            }
            else
            {
                return ServiceResult<AppSuperAdminUserDto>.Failure(
                    message: $"Bulunamayan roller: {string.Join(", ", notFoundRoles)}",
                    statusCode: (int)HttpStatusCode.BadRequest);
            }


            // DTO oluştur
            var resultDto = new AppSuperAdminUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                IsDefaultAdmin = user.IsDefaultAdmin,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                IsDeleted = user.IsDeleted,
                AccessFailedCount = user.AccessFailedCount,
                ConcurrencyStamp = user.ConcurrencyStamp,
                JobTitle = user.JobTitle,
                LockoutEnabled = user.LockoutEnabled,
                LockoutEnd = user.LockoutEnd,
                NormalizedEmail = user.NormalizedEmail,
                NormalizedUserName = user.NormalizedUserName,
                PasswordHash = user.PasswordHash,
                SecurityStamp = user.SecurityStamp,
                TwoFactorEnabled = user.TwoFactorEnabled
            };

            return ServiceResult<AppSuperAdminUserDto>.Success(
                data: resultDto,
                message: "Kullanıcı başarıyla oluşturuldu.",
                statusCode: (int)HttpStatusCode.Created);
        }
        public async Task<ServiceResult<AppSuperAdminUserDto>> UpdateUserAsync(AppSuperAdminUserEditDto userDto)
        {
            if (userDto == null || userDto.UserId <= 0)
            {
                return ServiceResult<AppSuperAdminUserDto>.Failure(
                    message: "Geçersiz kullanıcı bilgisi.",
                    statusCode: (int)HttpStatusCode.BadRequest);
            }

            if (userDto.IsDefaultAdmin)
            {
                return ServiceResult<AppSuperAdminUserDto>.Failure(
                   message: "Default Admin Kullanıcısı üzerinde değişiklik yapılamaz.",
                   statusCode: (int)HttpStatusCode.NotFound);
            }


            var validationResult = _validatorEdit.Validate(userDto);
            if (!validationResult.IsValid)
            {
                return ServiceResult<AppSuperAdminUserDto>.Failure(
                    message: "Geçersiz giriş verisi.",
                    statusCode: (int)HttpStatusCode.BadRequest,
                    validationErrors: validationResult.ToValidationDictionary());
            }

            // Kullanıcıyı bul
            var user = await _userManager.FindByIdAsync(userDto.UserId.ToString());
            if (user == null)
            {
                return ServiceResult<AppSuperAdminUserDto>.Failure(
                    message: "Kullanıcı bulunamadı.",
                    statusCode: (int)HttpStatusCode.NotFound);
            }

            // Bilgileri güncelle
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.UserName = userDto.UserName;
            user.Email = userDto.Email;
            user.PhoneNumber = userDto.PhoneNumber;
            user.EmailConfirmed = userDto.EmailConfirmed;
            user.PhoneNumberConfirmed = userDto.PhoneNumberConfirmed;
            user.IsDefaultAdmin = userDto.IsDefaultAdmin;
            user.JobTitle = userDto.JobTitle;
            user.LockoutEnabled = userDto.LockoutEnabled;
            user.LockoutEnd = userDto.LockoutEnd;
            user.ConcurrencyStamp = Guid.NewGuid().ToString(); // ConcurrencyStamp güncelle


            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return ServiceResult<AppSuperAdminUserDto>.Failure(
                    message: "Kullanıcı güncellenemedi.",
                    statusCode: (int)HttpStatusCode.BadRequest,
                    validationErrors: updateResult.Errors.ToValidationDictionary());
            }

            // 🔁 Rol güncelleme
            if (!string.IsNullOrWhiteSpace(userDto.SelectedRole))
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                var currentRole = currentRoles.FirstOrDefault();


                if (currentRole != null)
                    await _userManager.RemoveFromRoleAsync(user, currentRole);

                await _userManager.AddToRoleAsync(user, userDto.SelectedRole);
            }
            else
            {
                return ServiceResult<AppSuperAdminUserDto>.Failure(
                          message: $"'{userDto.SelectedRole}' adlı rol bulunamadı.",
                          statusCode: (int)HttpStatusCode.BadRequest);
            }

            // DTO oluştur
            var resultDto = new AppSuperAdminUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                IsDefaultAdmin = user.IsDefaultAdmin,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                IsDeleted = user.IsDeleted,
                AccessFailedCount = user.AccessFailedCount,
                ConcurrencyStamp = user.ConcurrencyStamp,
                JobTitle = user.JobTitle,
                LockoutEnabled = user.LockoutEnabled,
                LockoutEnd = user.LockoutEnd,
                NormalizedEmail = user.NormalizedEmail,
                NormalizedUserName = user.NormalizedUserName,
                PasswordHash = user.PasswordHash,
                SecurityStamp = user.SecurityStamp,
                TwoFactorEnabled = user.TwoFactorEnabled,
                RolesName = userDto.SelectedRole
            };

            return ServiceResult<AppSuperAdminUserDto>.Success(
                data: resultDto,
                message: "Kullanıcı başarıyla güncellendi.",
                statusCode: (int)HttpStatusCode.OK);
        }
        public async Task<ServiceResult<AppSuperAdminUserDto>> DeleteUserAsync(int Id)
        {
            var entity = _superAdminRepository.GetForEdit(x => x.Id == Id && !x.IsDeleted);
            if (entity == null)
            {
                return ServiceResult<AppSuperAdminUserDto>.Failure(
                    message: "Kullanıcı bulunamadı.",
                    statusCode: (int)HttpStatusCode.NotFound);
            }

            if (entity.IsDefaultAdmin)
            {
                return ServiceResult<AppSuperAdminUserDto>.Failure(
                   message: "Default Admin Kullanıcısı Silinemez.",
                   statusCode: (int)HttpStatusCode.NotFound);
            }

            _superAdminRepository.Delete(entity);
            await _unitOfWork.SaveDefaultChangesAsync();

            var dto = new AppSuperAdminUserDto
            {
                Id = entity.Id,
                UserName = entity.UserName,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                IsDefaultAdmin = entity.IsDefaultAdmin,
                IsDeleted = entity.IsDeleted,
                JobTitle = entity.JobTitle,
                AccessFailedCount = entity.AccessFailedCount,
                ConcurrencyStamp = entity.ConcurrencyStamp,
                EmailConfirmed = entity.EmailConfirmed,
                LockoutEnabled = entity.LockoutEnabled,
                LockoutEnd = entity.LockoutEnd,
                NormalizedEmail = entity.NormalizedEmail,
                NormalizedUserName = entity.NormalizedUserName,
                PasswordHash = entity.PasswordHash,
                PhoneNumberConfirmed = entity.PhoneNumberConfirmed,
                SecurityStamp = entity.SecurityStamp,
                TwoFactorEnabled = entity.TwoFactorEnabled
            };

            return ServiceResult<AppSuperAdminUserDto>.Success(
                data: dto,
                message: "Kullanıcı başarıyla silindi.",
                statusCode: (int)HttpStatusCode.OK);
        }
        public async Task<ServiceResult<NoContent>> ChangePasswordAsync(AppSuperAdminChangePasswordDto model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId.ToString());
            if (user == null)
            {
                return ServiceResult<NoContent>.Failure(
                    "Kullanıcı bulunamadı.",
                    statusCode: StatusCodes.Status404NotFound
                );
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                return ServiceResult<NoContent>.Success(null,
                    message: "Şifreniz başarıyla değiştirildi."
                );
            }

            var validationErrors = new ReadOnlyDictionary<string, string[]>(
                result.Errors
                    .GroupBy(e => e.Code)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.Description).ToArray()
                    )
            );

            return ServiceResult<NoContent>.Failure(
                "Şifre değiştirme sırasında hatalar oluştu.",
                errors: result.Errors.Select(e => e.Description),
                statusCode: StatusCodes.Status400BadRequest,
                validationErrors: validationErrors
            );
        }
    }

}
