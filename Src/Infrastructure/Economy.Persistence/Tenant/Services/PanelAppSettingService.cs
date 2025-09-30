using AutoMapper;
using Economy.Application.TenantUI.Dtos.AppSettingDtos;
using Economy.Application.TenantUI.Dtos.AppTechnicalSettingDtos;
using Economy.Application.TenantUI.Interfaces;
using Economy.Core.Interfaces;
using Economy.Core.Tools;
using Economy.Core.Tools.Result;
using Economy.Domain.Entites.EntityAppSettings;
using Economy.Panel.Application.Extensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Economy.Persistence.Tenant.Services
{
    public class PanelAppSettingService : IPanelAppSettingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityRepository<AppSetting, int> _appSettingRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<AppSettingCreateEditDto> _validator;
        public PanelAppSettingService(IUnitOfWork unitOfWork, IMapper mapper, IValidator<AppSettingCreateEditDto> validator)
        {
            _unitOfWork = unitOfWork;
            _appSettingRepository = unitOfWork.HotelEntityRepository<AppSetting>();
            _mapper = mapper;
            _validator = validator;
        }

        public ServiceResult<AppSettingDto> DeleteAppSetting(int Id)
        {
            var result = _appSettingRepository.GetForEdit(x => x.Id == Id, x => x.Translations);
            if (result == null)
            {
                return ServiceResult<AppSettingDto>.Empty("Herhangi bir kayıt bulunamadı.");
            }
            result.IsDeleted = true;
            _appSettingRepository.Update(result);
            _unitOfWork.SaveHotelChanges();

           var entity = _mapper.Map<AppSettingDto>(result);
            return ServiceResult<AppSettingDto>.Success(entity);
        }

        public ServiceResult<AppSettingDto> GetAppSetting(bool isDeleted)
        {
            var entity = _appSettingRepository.DataSet.Include(x=>x.Translations).Where(x => x.IsDeleted == isDeleted)
                                .OrderByDescending(x => x.Id)
                                .FirstOrDefault();

            if (entity == null)
                return ServiceResult<AppSettingDto>.Empty("Herhangi bir kayıt bulunamadı.");

            var dto = _mapper.Map<AppSettingDto>(entity);
            return ServiceResult<AppSettingDto>.Success(dto);

        }

        public ServiceResult<AppSettingDto> SaveAppSetting(AppSettingCreateEditDto appSettingDto)
        {

            var validation = _validator.Validate(appSettingDto);
            if (!validation.IsValid)
            {
                return ServiceResult<AppSettingDto>.Failure(
                    "Doğrulama hatası",
                    validationErrors: validation.ToValidationDictionary()
                );
            }

            AppSetting? entity;

            if (appSettingDto.Id > 0)
            {
                entity = _appSettingRepository.DataSet.FirstOrDefault(x => x.Id == appSettingDto.Id);
                if (entity == null)
                    return ServiceResult<AppSettingDto>.Failure("Kayıt bulunamadı.");

                _mapper.Map(appSettingDto, entity);
                _appSettingRepository.Update(entity);
            }
            else
            {
                entity = _mapper.Map<AppSetting>(appSettingDto);
                _appSettingRepository.Add(entity);
            }

            _unitOfWork.SaveHotelChanges();

            var dto = _mapper.Map<AppSettingDto>(entity);
            return ServiceResult<AppSettingDto>.Success(dto, "Kayıt başarılı.");



            // Var olan modeli al
            //var controlModel = _appSettingRepository.GetForEdit(w => w.Id == appSettingDto.Id, x => x.Translations);

            //if (controlModel == null)
            //{
            //    // Yeni model ekleme
            //    var newModel = new AppSetting
            //    {
            //        Id = appSettingDto.Id,
            //        Translations = new List<AppSettingTranslation>()
            //    };

            //    // Yeni çevirileri ekle
            //    foreach (var item in appSettingDto.Translations)
            //    {
            //        newModel.Translations.Add(new AppSettingTranslation
            //        {
            //            Description = item.Description,
            //            MetaDescription = item.MetaDescription,
            //            AppLanguageId = (int)item.AppLanguageId,
            //            AppSettingId = (int)item.AppSettingId,
            //            SiteTitle = item.SiteTitle,
            //            MetaTitle = item.MetaTitle
            //        });
            //    }

            //    // Yeni modeli ekle
            //    _appSettingRepository.Add(newModel);
            //}
            //else
            //{
            //    // Çeviriler güncelleniyor veya ekleniyor
            //    foreach (var item in appSettingDto.Translations)
            //    {
            //        // Mevcut çeviriyi bul
            //        var existingTranslation = controlModel.Translations
            //            .FirstOrDefault(t => t.AppLanguageId == item.AppLanguageId);

            //        if (existingTranslation != null)
            //        {
            //            // Var olan çeviriyi güncelle
            //            existingTranslation.Description = item.Description;
            //            existingTranslation.MetaDescription = item.MetaDescription;
            //            existingTranslation.SiteTitle = item.SiteTitle;
            //            existingTranslation.MetaTitle = item.MetaTitle;
            //        }
            //        else
            //        {
            //            // Yeni çeviri ekle
            //            controlModel.Translations.Add(new AppSettingTranslation
            //            {
            //                Description = item.Description,
            //                MetaDescription = item.MetaDescription,
            //                AppLanguageId = (int)item.AppLanguageId,
            //                AppSettingId = (int)item.AppSettingId,
            //                SiteTitle = item.SiteTitle,
            //                MetaTitle = item.MetaTitle
            //            });
            //        }
            //    }

            //    // Mevcut modeli güncelle
            //    _appSettingRepository.Update(controlModel);
            //}

            //// Değişiklikleri kaydet
            //_unitOfWork.SaveHotelChanges();

            //return ServiceResult<AppSettingDto>.Success(new AppSettingDto { Id = appSettingDto.Id });

        }


    }

}
