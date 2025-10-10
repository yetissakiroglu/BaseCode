using Economy.Base.Application.Dtos.BaseModels;
using Economy.Core.Interfaces;
using Economy.Core.Tools.Result;
using Economy.Domain.Entites.AppEntities;
using Economy.Domain.Entites.Identities;
using Economy.Domain.Entities.Identity;
using Economy.Panel.Application.Interfaces;
using Economy.Persistence.Repositories.AppBase.EntityFramework;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Economy.Panel.Persistence.Services
{
    public class PanelAppUserService : IPanelAppUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityRepository<AppUserToken, int> _appUserTokenRepository;
        private readonly IEntityRepository<AppUser, int> _appUserRepository;

        private readonly IValidator<AppUserCreateDto> _validator;
        private readonly IEntityRepository<App, int> _appRepository;

        private readonly UserManager<AppUser> _userManager;
        public PanelAppUserService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IValidator<AppUserCreateDto> validator)
        {
            _unitOfWork = unitOfWork;
            _appUserTokenRepository = unitOfWork.DefaultEntityRepository<AppUserToken>();
            _appUserRepository = unitOfWork.DefaultEntityRepository<AppUser>();
            _appRepository = unitOfWork.DefaultEntityRepository<App>();
            _userManager = userManager;
            _validator = validator;
        }

        public async Task<ServiceResult<List<AppUserDto>>> GetAllManagers()
        {
            if (_appUserRepository is not EfEntityRepositoryBase<AppUser> efRepo)
            {
                return ServiceResult<List<AppUserDto>>.Failure(
                    message: "Kullanıcı veri kaynağına erişilemedi.",
                    statusCode: (int)HttpStatusCode.InternalServerError
                );
            }

            var users = await efRepo.DataSet
                .AsNoTracking()
                .Where(x=>!x.IsDeleted)
                .Select(x => new AppUserDto
                {
                    Id = x.Id,
                    IsDefaultAdmin = x.IsDefaultAdmin,
                    Email = x.Email,
                    IsDeleted = x.IsDeleted,    
                    FirstName = x.FirstName,
                    JobTitle = x.JobTitle,
                    EmailConfirmed = x.EmailConfirmed,
                    LastName= x.LastName,
                    AccessFailedCount = x.AccessFailedCount,
                    ConcurrencyStamp = x.ConcurrencyStamp,
                    LockoutEnabled=x.LockoutEnabled,
                    LockoutEnd = x.LockoutEnd,
                    NormalizedEmail = x.NormalizedEmail,
                    NormalizedUserName = x.NormalizedUserName,
                    PasswordHash = x.PasswordHash,
                    PhoneNumber = x.PhoneNumber,
                    PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                    Roles = new List<string>(),
                    SecurityStamp = x.SecurityStamp,
                    TwoFactorEnabled = x.TwoFactorEnabled,
                    UserName = x.UserName
                  
                })
                .ToListAsync();

            if (!users.Any())
            {
                return ServiceResult<List<AppUserDto>>.Empty(
                    message: "Yönetici bulunamadı.",
                    statusCode: (int)HttpStatusCode.NoContent
                );
            }

            return ServiceResult<List<AppUserDto>>.Success(
                data: users,
                message: "Yöneticiler başarıyla getirildi.",
                statusCode: (int)HttpStatusCode.OK
            );
        }
        public async Task<ServiceResult<AppUserDto>> GetUser(int id, bool isDeleted)
        {
            var entity = _appUserRepository.GetForRead(w => w.Id == id && w.IsDeleted == isDeleted);
            if (entity is null)
            {
                return ServiceResult<AppUserDto>.Empty(
                    message: $"ID'si {id} olan kayıt bulunamadı.",
                    statusCode: (int)HttpStatusCode.NotFound
                );
            }

            var role =await _userManager.GetRolesAsync(entity);


            var dto = new AppUserDto
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                UserName = entity.UserName,
                //TenantId = entity.TenantId,
                IsDefaultAdmin = entity.IsDefaultAdmin,
                JobTitle = entity.JobTitle,
                Roles = role?.ToList() ?? new List<string>()
            };

            return ServiceResult<AppUserDto>.Success(
                data: dto,
                message: "Kaydı başarıyla getirildi.",
                statusCode: (int)HttpStatusCode.OK
            );

        }
     
    }
}
