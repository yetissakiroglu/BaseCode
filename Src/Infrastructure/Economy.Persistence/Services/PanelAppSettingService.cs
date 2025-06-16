using Economy.Core.Interfaces;
using Economy.Core.Tools;
using Economy.Domain.Entites.EntityAppSettings;
using Economy.Panel.Application.Dtos.AppSettingDtos;
using Economy.Panel.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Economy.Panel.Persistence.Services
{
    public class PanelAppSettingService : IPanelAppSettingService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityRepository<AppSetting, int> _appSettingRepository;

        public PanelAppSettingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _appSettingRepository = unitOfWork.EntityRepository<AppSetting>();
        }

        public ResponseModel<AppSettingDto> DeleteAppSetting(int Id)
        {
            var result = _appSettingRepository.GetForEdit(x => x.Id == Id, x => x.Translations);
            if (result == null)
            {
                return ResponseModel<AppSettingDto>.Fail("Kayıt bulunamadı.", HttpStatusCode.NotFound);
            }
            result.IsDeleted = true;
            _appSettingRepository.Update(result);
            _unitOfWork.SaveHotelChanges();

            var appSettingDto = new AppSettingDto
            {
                Id = result.Id,
                Translations = result.Translations.Select(x => new AppSettingTranslationDto
                {
                    Id = x.Id,
                    AppSettingId = x.AppSettingId,
                    AppLanguageId = x.AppLanguageId,
                    SiteTitle = x.SiteTitle,
                    Description = x.Description,
                    MetaTitle = x.MetaTitle,
                    MetaDescription = x.MetaDescription,
                }).ToList()
            };

            return ResponseModel<AppSettingDto>.Success(appSettingDto, HttpStatusCode.OK);
        }

        public ResponseModel<AppSettingDto> GetAppSetting(bool isDeleted)
        {

            var result = _appSettingRepository.GetForReadFunc(
                x => x.IsDeleted == isDeleted,
                x => x.Include(y => y.Translations)
            );

            if (result == null)
            {
                return ResponseModel<AppSettingDto>.Fail("Kayıt bulunamadı.", HttpStatusCode.NotFound);
            }

            var appSettingDto = new AppSettingDto
            {
                Id = result.Id,
                Translations = result.Translations.Select(x => new AppSettingTranslationDto
                {
                    Id = x.Id,
                    AppSettingId = x.AppSettingId,
                    AppLanguageId = x.AppLanguageId,
                    SiteTitle = x.SiteTitle,
                    Description = x.Description,
                    MetaTitle = x.MetaTitle,
                    MetaDescription = x.MetaDescription,
                }).ToList()
            };

            return ResponseModel<AppSettingDto>.Success(appSettingDto, HttpStatusCode.OK);

        }

        public ResponseModel<AppSettingDto> SaveAppSetting(AppSettingCreateEditDto appSettingDto)
        {
            // Var olan modeli al
            var controlModel = _appSettingRepository.GetForEdit(w => w.Id == appSettingDto.Id, x => x.Translations);

            if (controlModel == null)
            {
                // Yeni model ekleme
                var newModel = new AppSetting
                {
                    Id = appSettingDto.Id,
                    Translations = new List<AppSettingTranslation>()
                };

                // Yeni çevirileri ekle
                foreach (var item in appSettingDto.Translations)
                {
                    newModel.Translations.Add(new AppSettingTranslation
                    {
                        Description = item.Description,
                        MetaDescription = item.MetaDescription,
                        AppLanguageId = (int)item.AppLanguageId,
                        AppSettingId = (int)item.AppSettingId,
                        SiteTitle = item.SiteTitle,
                        MetaTitle = item.MetaTitle
                    });
                }

                // Yeni modeli ekle
                _appSettingRepository.Add(newModel);
            }
            else
            {
                // Çeviriler güncelleniyor veya ekleniyor
                foreach (var item in appSettingDto.Translations)
                {
                    // Mevcut çeviriyi bul
                    var existingTranslation = controlModel.Translations
                        .FirstOrDefault(t => t.AppLanguageId == item.AppLanguageId);

                    if (existingTranslation != null)
                    {
                        // Var olan çeviriyi güncelle
                        existingTranslation.Description = item.Description;
                        existingTranslation.MetaDescription = item.MetaDescription;
                        existingTranslation.SiteTitle = item.SiteTitle;
                        existingTranslation.MetaTitle = item.MetaTitle;
                    }
                    else
                    {
                        // Yeni çeviri ekle
                        controlModel.Translations.Add(new AppSettingTranslation
                        {
                            Description = item.Description,
                            MetaDescription = item.MetaDescription,
                            AppLanguageId = (int)item.AppLanguageId,
                            AppSettingId = (int)item.AppSettingId,
                            SiteTitle = item.SiteTitle,
                            MetaTitle = item.MetaTitle
                        });
                    }
                }

                // Mevcut modeli güncelle
                _appSettingRepository.Update(controlModel);
            }

            // Değişiklikleri kaydet
            _unitOfWork.SaveHotelChanges();

            return ResponseModel<AppSettingDto>.Success(new AppSettingDto { Id = appSettingDto.Id }, HttpStatusCode.OK);

        }


    }

}
