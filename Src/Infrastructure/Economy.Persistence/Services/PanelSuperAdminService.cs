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
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Economy.Persistence.Services
{
    public class PanelSuperAdminService : IPanelSuperAdminService
    {
        private readonly IEntityRepository<AppUser, int> _superAdminRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IValidator<AppSuperAdminUserCreateDto> _validatorCreate;
        private readonly IValidator<AppSuperAdminUserEditDto> _validatorEdit;

        public PanelSuperAdminService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IValidator<AppSuperAdminUserCreateDto> validatorCreate, IValidator<AppSuperAdminUserEditDto> validatorEdit)
        {
            _unitOfWork = unitOfWork;
            _superAdminRepository = unitOfWork.DefaultEntityRepository<AppUser>();
            _userManager = userManager;
            _roleManager = roleManager;
            _validatorCreate = validatorCreate;
            _validatorEdit = validatorEdit;
        }
        public async Task<ServiceResult<List<AppRoleDto>>> GetRolesAsync()
        {
            try
            {
                var roles = await Task.FromResult(
                    _roleManager.Roles
                        .Select(role => new AppRoleDto
                        {
                            Name = role.Name
                        }).ToList()
                );

                if (roles.Any())
                    return ServiceResult<List<AppRoleDto>>.Success(roles, "Roller başarıyla alındı.");
                else
                    return ServiceResult<List<AppRoleDto>>.Empty("Kayıtlı rol bulunamadı.");
            }
            catch (Exception ex)
            {
                return ServiceResult<List<AppRoleDto>>.Failure(
                    "Roller alınırken bir hata oluştu.",
                    new[] { ex.Message }
                );
            }
        }
        public async Task<ServiceResult<AppSuperAdminUserDto>> CreateUserAsync(AppSuperAdminUserCreateDto userDto)
        {
            if (userDto == null)
            {
                return ServiceResult<AppSuperAdminUserDto>.Failure(
                    message: "Geçersiz veri gönderildi.",
                    statusCode: (int)HttpStatusCode.BadRequest);
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
                PhotoUrl = userDto.PhotoUrl,
                JobTitle = userDto.JobTitle,
                TwoFactorEnabled = userDto.TwoFactorEnabled,
                LockoutEnabled = userDto.LockoutEnabled
            };

            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (!result.Succeeded)
            {
                return ServiceResult<AppSuperAdminUserDto>.Failure(
                    message: "Kullanıcı oluşturulamadı.",
                    statusCode: (int)HttpStatusCode.BadRequest,
                    validationErrors: result.Errors.ToValidationDictionary());
            }

            // 🔽 Rol Ataması
            if (!string.IsNullOrWhiteSpace(userDto.RoleName))
            {
                var roleExists = await _roleManager.RoleExistsAsync(userDto.RoleName);
                if (roleExists)
                {
                    await _userManager.AddToRoleAsync(user, userDto.RoleName);
                }
                else
                {
                    return ServiceResult<AppSuperAdminUserDto>.Failure(
                        message: $"'{userDto.RoleName}' adlı rol bulunamadı.",
                        statusCode: (int)HttpStatusCode.BadRequest);
                }
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
                PhotoUrl = user.PhotoUrl,
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
            user.PhotoUrl = userDto.PhotoUrl;
            user.JobTitle = userDto.JobTitle;
            user.TwoFactorEnabled = userDto.TwoFactorEnabled;
            user.LockoutEnabled = userDto.LockoutEnabled;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return ServiceResult<AppSuperAdminUserDto>.Failure(
                    message: "Kullanıcı güncellenemedi.",
                    statusCode: (int)HttpStatusCode.BadRequest,
                    validationErrors: updateResult.Errors.ToValidationDictionary());
            }

            // 🔁 Rol güncelleme
            if (!string.IsNullOrWhiteSpace(userDto.RoleName))
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                var currentRole = currentRoles.FirstOrDefault();

                if (currentRole != userDto.RoleName)
                {
                    if (currentRole != null)
                        await _userManager.RemoveFromRoleAsync(user, currentRole);

                    var roleExists = await _roleManager.RoleExistsAsync(userDto.RoleName);
                    if (roleExists)
                    {
                        await _userManager.AddToRoleAsync(user, userDto.RoleName);
                    }
                    else
                    {
                        return ServiceResult<AppSuperAdminUserDto>.Failure(
                            message: $"'{userDto.RoleName}' adlı rol bulunamadı.",
                            statusCode: (int)HttpStatusCode.BadRequest);
                    }
                }
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
                PhotoUrl = user.PhotoUrl,
                SecurityStamp = user.SecurityStamp,
                TwoFactorEnabled = user.TwoFactorEnabled
            };

            return ServiceResult<AppSuperAdminUserDto>.Success(
                data: resultDto,
                message: "Kullanıcı başarıyla güncellendi.",
                statusCode: (int)HttpStatusCode.OK);
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
            var roleName = roles.FirstOrDefault(); // Çoklu rol varsa sadece ilkini alıyoruz

            var dto = new AppSuperAdminUserDto
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                UserName = entity.UserName,
                Email = entity.Email,
                IsDeleted = entity.IsDeleted,
                IsDefaultAdmin = entity.IsDefaultAdmin,
                PhotoUrl = entity.PhotoUrl,
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

                // ✅ Yeni eklenen alan
                RoleName = roleName
            };

            return ServiceResult<AppSuperAdminUserDto>.Success(dto);
        }
        public async Task<ServiceResult<List<AppSuperAdminUserDto>>> GetUserListAsync()
        {
            var list = _superAdminRepository
                .WhereForRead(x => !x.IsDeleted)
                .Select(x => new AppSuperAdminUserDto
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserName = x.UserName,
                    Email = x.Email,
                    IsDeleted = x.IsDeleted,
                    IsDefaultAdmin = x.IsDefaultAdmin,
                    PhotoUrl = x.PhotoUrl,
                    JobTitle = x.JobTitle,
                    PhoneNumber = x.PhoneNumber,
                    TwoFactorEnabled = x.TwoFactorEnabled,
                    AccessFailedCount = x.AccessFailedCount,
                    ConcurrencyStamp = x.ConcurrencyStamp,
                    EmailConfirmed = x.EmailConfirmed,
                    LockoutEnabled = x.LockoutEnabled,
                    LockoutEnd = x.LockoutEnd,
                    NormalizedEmail = x.NormalizedEmail,
                    NormalizedUserName = x.NormalizedUserName,
                    PasswordHash = x.PasswordHash,
                    PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                    SecurityStamp = x.SecurityStamp,
                    
                })
                .ToList();

            return ServiceResult<List<AppSuperAdminUserDto>>.Success(list);
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
                PhotoUrl = entity.PhotoUrl,
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
                // Rol bilgisi gerekiyorsa ayrıca çekilebilir
            };

            return ServiceResult<AppSuperAdminUserDto>.Success(
                data: dto,
                message: "Kullanıcı başarıyla silindi.",
                statusCode: (int)HttpStatusCode.OK);
        }
    }

}
