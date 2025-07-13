using Economy.Core.Helpers;
using Economy.Core.Interfaces;
using Economy.Core.Tools;
using Economy.Core.Tools.Result;
using Economy.Domain.Entites.EntityAppSettings;
using Economy.Panel.Application.Dtos.AppSettingLogoDtos;
using Economy.Panel.Application.Interfaces;
using System.Net;

namespace Economy.Panel.Persistence.Services
{
    public class PanelAppSettingLogoService : IPanelAppSettingLogoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityRepository<AppSettingLogo, int> _appSettingLogoRepository;
        private readonly IFileImageHelperService _fileImageHelperService;

        public PanelAppSettingLogoService(IUnitOfWork unitOfWork, IFileImageHelperService fileImageHelperService)
        {
            _unitOfWork = unitOfWork;
            _appSettingLogoRepository = unitOfWork.HotelEntityRepository<AppSettingLogo>();
            _fileImageHelperService = fileImageHelperService;
        }

        public ServiceResult<AppSettingLogoDto> CreateEditAppSettingLogo(AppSettingLogoCreateEditDto model)
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

                if (model.LogoBase64 is not null)
                {
                    var webImage = _fileImageHelperService.UploadBase64(model.LogoBase64, new List<string> { "updates", "logo" });
                    newModel.LogoPath = webImage.Data.MediaFullURL;
                }

                if (model.MobileLogoBase64 is not null)
                {
                    var mobilImage = _fileImageHelperService.UploadBase64(model.MobileLogoBase64, new List<string> { "updates", "logo" });
                    newModel.MobileLogoPath = mobilImage.Data.MediaFullURL;
                }
                if (model.FaviconBase64 is not null)
                {
                    var mobilImage = _fileImageHelperService.UploadBase64(model.FaviconBase64, new List<string> { "updates", "logo" });
                    newModel.FaviconPath = mobilImage.Data.MediaFullURL;
                }

                // Yeni modeli ekle
                _appSettingLogoRepository.Add(newModel);
            }
            else
            {
           
                if (model.LogoBase64 is not null)
                {
                    var webImage = _fileImageHelperService.UploadBase64(model.LogoBase64, new List<string> { "updates", "logo" });
                    controlModel.LogoPath = webImage.Data.MediaFullURL;
                }

                if (model.MobileLogoBase64 is not null)
                {
                    var mobilImage = _fileImageHelperService.UploadBase64(model.MobileLogoBase64, new List<string> { "updates", "logo" });
                    controlModel.MobileLogoPath = mobilImage.Data.MediaFullURL;
                }
                if (model.FaviconBase64 is not null)
                {
                    var mobilImage = _fileImageHelperService.UploadBase64(model.FaviconBase64, new List<string> { "updates", "logo" });
                    controlModel.FaviconPath = mobilImage.Data.MediaFullURL;
                }

                // Mevcut modeli güncelle
                _appSettingLogoRepository.Update(controlModel);
            }

            // Değişiklikleri kaydet
            _unitOfWork.SaveHotelChanges();

            return ServiceResult<AppSettingLogoDto>.Success(new AppSettingLogoDto { Id = model.Id });
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
