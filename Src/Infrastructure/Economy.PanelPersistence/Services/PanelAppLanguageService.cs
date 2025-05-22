using Economy.Core.Tools.Result;
using Economy.Domain.Entites.EntityAppLanguage;
using Economy.Panel.Application.Dtos.AppLanguageDtos;
using Economy.Panel.Application.Extensions;
using Economy.Panel.Application.Interfaces;
using Economy.Panel.Application.Validations.AppLanguageValidator;
using FluentValidation.Results;
using System.Net;

namespace Economy.Core.Interfaces.Economy.Panel.Persistence.Services
{
    public class PanelAppLanguageService : IPanelAppLanguageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityRepository<AppLanguage, int> _appLanguageRepository;
        public PanelAppLanguageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _appLanguageRepository = unitOfWork.EntityRepository<AppLanguage>();
        }
        public ServiceResult<AppLanguageDto> CreateLanguage(AppLanguageCreateEditDto model)
        {
            var validator = new AppLanguageCreateEditDtoValidator();
            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                return ServiceResult<AppLanguageDto>.Failure(
                           message: "Geçersiz giriş verisi.",
                           statusCode: (int)HttpStatusCode.BadRequest,
                           validationErrors: validationResult.ToValidationDictionary());
            }

            var entity = new AppLanguage
            {
                Code = model.Code,
                Name = model.Name,
                IsRTL = model.IsRTL,
                Icon = model.Icon,
                IsActive = model.IsActive,
                IsDefault = model.IsDefault,
            };

            _appLanguageRepository.Add(entity);
            _unitOfWork.SaveHotelChanges();

            var dto = new AppLanguageDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Code = entity.Code,
                Icon = entity.Icon,
                IsActive = entity.IsActive,
                IsDefault = entity.IsDefault,
                IsRTL = entity.IsRTL
            };

            return ServiceResult<AppLanguageDto>.Success(
                dto,
                message: "Dil başarıyla oluşturuldu.",
                statusCode: (int)HttpStatusCode.Created
            );
        }
        public ServiceResult<AppLanguageDto> DeleteLanguage(int id)
        {
            // Veritabanından silinecek dili bul
            var entity = _appLanguageRepository.GetForRead(w => w.Id == id);

            if (entity is null)
            {
                return ServiceResult<AppLanguageDto>.Failure(
                    message: "Dil bulunamadı.",
                    statusCode: (int)HttpStatusCode.NotFound
                );
            }

            // Silme işlemi (soft delete)
            entity.IsDeleted = true;

            _appLanguageRepository.Update(entity);
            _unitOfWork.SaveHotelChanges();

            // DTO'ya dönüştür
            var dto = new AppLanguageDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Code = entity.Code,
                Icon = entity.Icon,
                IsActive = entity.IsActive,
                IsDefault = entity.IsDefault,
                IsRTL = entity.IsRTL
            };

            return ServiceResult<AppLanguageDto>.Success(
                dto,
                message: "Dil başarıyla silindi.",
                statusCode: (int)HttpStatusCode.OK
            );
        }
        public ServiceResult<AppLanguageDto> EditLanguage(AppLanguageCreateEditDto model)
        {
            var validator = new AppLanguageCreateEditDtoValidator();
            ValidationResult validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                return ServiceResult<AppLanguageDto>.Failure(
                  message: "Doğrulama hatası oluştu.",
                  errors: validationResult.Errors.Select(e => e.ErrorMessage),
                  statusCode: (int)HttpStatusCode.BadRequest,
                  validationErrors: validationResult.ToValidationDictionary()
              );
            }

            var result = _appLanguageRepository.GetForRead(w => w.Id == model.Id);
            if (result is null)
                return ServiceResult<AppLanguageDto>.Failure(
                    message: "Dil bulunamadı.",
                    statusCode: (int)HttpStatusCode.NotFound
                );
            result.Id = model.Id;
            result.Name = model.Name;
            result.IsRTL = model.IsRTL;
            result.IsDefault = model.IsDefault;
            result.Code = model.Code;
            result.Icon = model.Icon;
            result.IsActive = model.IsActive;

            _appLanguageRepository.Update(result);
            _unitOfWork.SaveHotelChanges();

            var dto = new AppLanguageDto
            {
                Id = result.Id,
                Name = result.Name,
                Code = result.Code,
                Icon = result.Icon,
                IsActive = result.IsActive,
                IsDefault = result.IsDefault,
                IsRTL = result.IsRTL
            };

            return ServiceResult<AppLanguageDto>.Success(
                dto,
                message: "Dil başarıyla güncellendi.",
                statusCode: (int)HttpStatusCode.OK
            );
        }
        public ServiceResult<List<AppLanguageDto>> GetAllLanguage(bool isDeleted, bool isActive)
        {
            var languages = _appLanguageRepository
                .WhereForRead(w => w.IsDeleted == isDeleted && w.IsActive == isActive)
                .Select(x => new AppLanguageDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    Icon = x.Icon,
                    IsActive = x.IsActive,
                    IsDefault = x.IsDefault,
                    IsRTL = x.IsRTL
                })
                .ToList();

            if (languages.Count == 0)
            {
                return ServiceResult<List<AppLanguageDto>>.Empty(
                    message: "Dil Kayıtı bulunamadı.",
                    statusCode: (int)HttpStatusCode.NoContent
                );
            }

            return ServiceResult<List<AppLanguageDto>>.Success(
                data: languages,
                message: "Diller başarıyla getirildi.",
                statusCode: (int)HttpStatusCode.OK
            );
        }
        public ServiceResult<List<AppLanguageDto>> GetAllLanguage(bool isDeleted)
        {
            var languages = _appLanguageRepository
                 .WhereForRead(w => w.IsDeleted == isDeleted)
                 .Select(x => new AppLanguageDto
                 {
                     Id = x.Id,
                     Name = x.Name,
                     Code = x.Code,
                     Icon = x.Icon,
                     IsActive = x.IsActive,
                     IsDefault = x.IsDefault,
                     IsRTL = x.IsRTL
                 })
                 .ToList();

            if (languages.Count == 0)
            {
                return ServiceResult<List<AppLanguageDto>>.Empty(
                    message: "Kayıt bulunamadı.",
                    statusCode: (int)HttpStatusCode.NoContent
                );
            }

            return ServiceResult<List<AppLanguageDto>>.Success(
                data: languages,
                message: "Diller başarıyla getirildi.",
                statusCode: (int)HttpStatusCode.OK
            );
        }
        public ServiceResult<AppLanguageDto> GetLanguage(int id, bool isDeleted)
        {
            var entity = _appLanguageRepository.GetForRead(w => w.IsDeleted == isDeleted && w.Id == id);
            if (entity is null)
            {
                return ServiceResult<AppLanguageDto>.Empty(
                    message: $"ID'si {id} olan dil kaydı bulunamadı.",
                    statusCode: (int)HttpStatusCode.NotFound
                );
            }

            var dto = new AppLanguageDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Code = entity.Code,
                Icon = entity.Icon,
                IsActive = entity.IsActive,
                IsDefault = entity.IsDefault,
                IsRTL = entity.IsRTL
            };

            return ServiceResult<AppLanguageDto>.Success(
                data: dto,
                message: "Dil kaydı başarıyla getirildi.",
                statusCode: (int)HttpStatusCode.OK
            );
        }
    }
}
