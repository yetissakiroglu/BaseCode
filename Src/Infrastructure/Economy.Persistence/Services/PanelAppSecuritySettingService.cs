using Economy.Application.Dtos.AppSecuritySettingDtos;
using Economy.Application.Interfaces;
using Economy.Core.Interfaces;
using Economy.Core.Tools.Result;
using Economy.Domain.Entites.AppEntities;
using Economy.Panel.Application.Extensions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Persistence.Services
{

    public class PanelAppSecuritySettingService : IPanelAppSecuritySettingService
    {
        private readonly IUnitOfWork _uow;
        private readonly IEntityRepository<AppSecuritySetting, int> _repo;
        private readonly IValidator<AppSecuritySettingCreateDto> _vCreate;
        private readonly IValidator<AppSecuritySettingEditDto> _vEdit;

        public PanelAppSecuritySettingService(
            IUnitOfWork uow,
            IValidator<AppSecuritySettingCreateDto> vCreate,
            IValidator<AppSecuritySettingEditDto> vEdit)
        {
            _uow = uow;
            _repo = uow.DefaultEntityRepository<AppSecuritySetting>();
            _vCreate = vCreate;
            _vEdit = vEdit;
        }

        public async Task<ServiceResult<AppSecuritySettingDto>> CreateAsync(AppSecuritySettingCreateDto m)
        {
            var vr = _vCreate.Validate(m);
            if (!vr.IsValid)
                return ServiceResult<AppSecuritySettingDto>.Failure("Geçersiz veri.", statusCode:(int)HttpStatusCode.BadRequest, validationErrors: vr.ToValidationDictionary());

            var e = new AppSecuritySetting
            {
                PasswordRequiredLength = m.PasswordRequiredLength,
                PasswordRequireDigit = m.PasswordRequireDigit,
                PasswordRequireLowercase = m.PasswordRequireLowercase,
                PasswordRequireUppercase = m.PasswordRequireUppercase,
                PasswordRequireNonAlphanumeric = m.PasswordRequireNonAlphanumeric,
                LockoutTimeSpanMinutes = m.LockoutTimeSpanMinutes,
                LockoutMaxFailedAccessAttempts = m.LockoutMaxFailedAccessAttempts,
                LockoutAllowedForNewUsers = m.LockoutAllowedForNewUsers,
                SignInRequireConfirmedEmail = m.SignInRequireConfirmedEmail,
                SignInRequireConfirmedPhoneNumber = m.SignInRequireConfirmedPhoneNumber,
                TwoFactorRequired = m.TwoFactorRequired
            };

            _repo.Add(e);
            await _uow.SaveDefaultChangesAsync();

            return ServiceResult<AppSecuritySettingDto>.Success(Map(e), "Güvenlik ayarları oluşturuldu.", (int)HttpStatusCode.Created);
        }

        public async Task<ServiceResult<AppSecuritySettingDto>> UpdateAsync(AppSecuritySettingEditDto m)
        {
            var vr = _vEdit.Validate(m);
            if (!vr.IsValid)
                return ServiceResult<AppSecuritySettingDto>.Failure("Geçersiz veri.", statusCode: (int)HttpStatusCode.BadRequest, validationErrors: vr.ToValidationDictionary());

            var e = _repo.GetForEdit(x => x.Id == m.Id && !x.IsDeleted);
            if (e == null)
                return ServiceResult<AppSecuritySettingDto>.Failure("Kayıt bulunamadı.", statusCode: (int)HttpStatusCode.NotFound);

            e.PasswordRequiredLength = m.PasswordRequiredLength;
            e.PasswordRequireDigit = m.PasswordRequireDigit;
            e.PasswordRequireLowercase = m.PasswordRequireLowercase;
            e.PasswordRequireUppercase = m.PasswordRequireUppercase;
            e.PasswordRequireNonAlphanumeric = m.PasswordRequireNonAlphanumeric;
            e.LockoutTimeSpanMinutes = m.LockoutTimeSpanMinutes;
            e.LockoutMaxFailedAccessAttempts = m.LockoutMaxFailedAccessAttempts;
            e.LockoutAllowedForNewUsers = m.LockoutAllowedForNewUsers;
            e.SignInRequireConfirmedEmail = m.SignInRequireConfirmedEmail;
            e.SignInRequireConfirmedPhoneNumber = m.SignInRequireConfirmedPhoneNumber;
            e.TwoFactorRequired = m.TwoFactorRequired;

            _repo.Update(e);
            await _uow.SaveDefaultChangesAsync();

            return ServiceResult<AppSecuritySettingDto>.Success(Map(e), "Güvenlik ayarları güncellendi.", (int)HttpStatusCode.OK);
        }

        public Task<ServiceResult<AppSecuritySettingDto>> GetAsync(int id)
        {
            if (id <= 0)
                return Task.FromResult(ServiceResult<AppSecuritySettingDto>.Failure("Geçersiz Id.", statusCode:(int)HttpStatusCode.BadRequest));

            var e = _repo.GetForRead(x => x.Id == id && !x.IsDeleted);
            if (e == null)
                return Task.FromResult(ServiceResult<AppSecuritySettingDto>.Failure("Kayıt bulunamadı.", statusCode: (int)HttpStatusCode.NotFound));

            return Task.FromResult(ServiceResult<AppSecuritySettingDto>.Success(Map(e)));
        }

        public Task<ServiceResult<List<AppSecuritySettingDto>>> ListAsync()
        {
            var list = _repo.WhereForRead(x => !x.IsDeleted)
                .Select(x => Map(x))
                .ToList();

            return Task.FromResult(ServiceResult<List<AppSecuritySettingDto>>.Success(list));
        }

        public async Task<ServiceResult<AppSecuritySettingDto>> DeleteAsync(int id)
        {
            if (id <= 0)
                return ServiceResult<AppSecuritySettingDto>.Failure("Geçersiz Id.", statusCode: (int)HttpStatusCode.BadRequest);

            var e = _repo.GetForEdit(x => x.Id == id && !x.IsDeleted);
            if (e == null)
                return ServiceResult<AppSecuritySettingDto>.Failure("Kayıt bulunamadı.", statusCode:(int)HttpStatusCode.NotFound);

            _repo.Delete(e);
            await _uow.SaveDefaultChangesAsync();

            return ServiceResult<AppSecuritySettingDto>.Success(Map(e), "Kayıt silindi.", (int)HttpStatusCode.OK);
        }

        private static AppSecuritySettingDto Map(AppSecuritySetting e) => new()
        {
            Id = e.Id,
            PasswordRequiredLength = e.PasswordRequiredLength,
            PasswordRequireDigit = e.PasswordRequireDigit,
            PasswordRequireLowercase = e.PasswordRequireLowercase,
            PasswordRequireUppercase = e.PasswordRequireUppercase,
            PasswordRequireNonAlphanumeric = e.PasswordRequireNonAlphanumeric,
            LockoutTimeSpanMinutes = e.LockoutTimeSpanMinutes,
            LockoutMaxFailedAccessAttempts = e.LockoutMaxFailedAccessAttempts,
            LockoutAllowedForNewUsers = e.LockoutAllowedForNewUsers,
            SignInRequireConfirmedEmail = e.SignInRequireConfirmedEmail,
            SignInRequireConfirmedPhoneNumber = e.SignInRequireConfirmedPhoneNumber,
            TwoFactorRequired = e.TwoFactorRequired,

        };
    }
}
