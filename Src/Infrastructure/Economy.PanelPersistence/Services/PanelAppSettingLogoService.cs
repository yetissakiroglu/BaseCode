using Economy.Core.Interfaces;
using Economy.Core.Tools;
using Economy.Domain.Entites.EntityAppLanguage;
using Economy.Domain.Entites.EntityAppSettings;
using Economy.Panel.Application.Dtos.AppSettingDtos;
using Economy.Panel.Application.Dtos.AppSettingLogoDtos;
using Economy.Panel.Application.Interfaces;
using System.Net;

namespace Economy.Panel.Persistence.Services
{
    public class PanelAppSettingLogoService : IPanelAppSettingLogoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityRepository<AppSettingLogo, int> _appSettingLogoRepository;

        public PanelAppSettingLogoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _appSettingLogoRepository = unitOfWork.EntityRepository<AppSettingLogo>();
        }

        public ResponseModel<AppSettingLogoDto> CreateEditAppSettingLogo(AppSettingLogoCreateEditDto model)
        {
            // Var olan modeli al
            var controlModel = _appSettingLogoRepository.GetForEdit(w => w.Id == model.Id);

            if (controlModel == null)
            {
                // Yeni model ekleme
                var newModel = new AppSettingLogo
                {
                    Id = model.Id,
                    IsDeleted = false,
                    FaviconPath = model.FaviconPath,
                    LogoPath = model.LogoPath,
                    MobileLogoPath = model.MobileLogoPath
                };

                // Yeni modeli ekle
                _appSettingLogoRepository.Add(newModel);
            }
            else
            {
                controlModel.LogoPath = model.LogoPath;
                controlModel.MobileLogoPath = model.MobileLogoPath;
                controlModel.FaviconPath = model.FaviconPath;
                

                // Mevcut modeli güncelle
                _appSettingLogoRepository.Update(controlModel);
            }

            // Değişiklikleri kaydet
            _unitOfWork.SaveHotelChanges();

            return ResponseModel<AppSettingLogoDto>.Success(new AppSettingLogoDto { Id = model.Id }, HttpStatusCode.OK);
        }

        public ResponseModel<AppSettingLogoDto> DeleteAppSettingLogo(int id)
        {
            throw new NotImplementedException();
        }

        public ResponseModel<AppSettingLogoDto> GetAppSettingLogo(bool isDeleted)
        {
            var result = _appSettingLogoRepository.GetForRead(
                x => x.IsDeleted == isDeleted);       

            if (result == null)
            {
                return ResponseModel<AppSettingLogoDto>.Fail("Kayıt bulunamadı.", HttpStatusCode.NotFound);
            }
            var entityDto = new AppSettingLogoDto
            {
                Id = result.Id,
                FaviconPath = result.FaviconPath,
                LogoPath = result.LogoPath,
                MobileLogoPath = result.MobileLogoPath
            };

            return ResponseModel<AppSettingLogoDto>.Success(entityDto, HttpStatusCode.OK);
        }
    }
}
