using AutoMapper;
using Economy.Application.TenantUI.Dtos.AppSettingLogoDtos;
using Economy.Application.TenantUI.Interfaces;
using Economy.Core.Helpers;
using Economy.Core.Interfaces;
using Economy.Core.Tools;
using Economy.Core.Tools.Result;
using Economy.Domain.Entites.EntityAppSettings;
using Economy.Panel.Application.Extensions;
using FluentValidation;
using System.Net;

namespace Economy.Persistence.Tenant.Services
{
    public class PanelAppSettingLogoService : IPanelAppSettingLogoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityRepository<AppSettingLogo, int> _appSettingLogoRepository;
        private readonly IFileImageHelperService _fileImageHelperService;
        private readonly IMapper _mapper;
        private readonly IValidator<AppSettingLogoCreateEditDto> _validator;
        public PanelAppSettingLogoService(IUnitOfWork unitOfWork, IFileImageHelperService fileImageHelperService, IMapper mapper, IValidator<AppSettingLogoCreateEditDto> validator)
        {
            _unitOfWork = unitOfWork;
            _appSettingLogoRepository = unitOfWork.HotelEntityRepository<AppSettingLogo>();
            _fileImageHelperService = fileImageHelperService;
            _mapper = mapper;
            _validator = validator;
        }

        public ServiceResult<AppSettingLogoDto> CreateEditAppSettingLogo(AppSettingLogoCreateEditDto model)
        {
            var validation = _validator.Validate(model);
            if (!validation.IsValid)
            {
                return ServiceResult<AppSettingLogoDto>.Failure(
                    "Doğrulama hatası",
                    validationErrors: validation.ToValidationDictionary()
                );
            }

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
                if (model.ShareImageBase64 is not null)
                {
                    var mobilImage = _fileImageHelperService.UploadBase64(model.ShareImageBase64, new List<string> { "updates", "logo" });
                    newModel.ShareImagePath = mobilImage.Data.MediaFullURL;
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

                if (model.ShareImageBase64 is not null)
                {
                    var mobilImage = _fileImageHelperService.UploadBase64(model.ShareImageBase64, new List<string> { "updates", "logo" });
                    controlModel.ShareImagePath = mobilImage.Data.MediaFullURL;
                }

                // Mevcut modeli güncelle
                _appSettingLogoRepository.Update(controlModel);
            }

            // Değişiklikleri kaydet
            _unitOfWork.SaveHotelChanges();
            var entity = _mapper.Map<AppSettingLogoDto>(controlModel);

            return ServiceResult<AppSettingLogoDto>.Success(entity);
        }
        public ServiceResult<AppSettingLogoDto> GetAppSettingLogo(bool isDeleted)
        {
            var result = _appSettingLogoRepository.GetForRead(
                x => x.IsDeleted == isDeleted);       

            if (result == null)
            {
                return ServiceResult<AppSettingLogoDto>.Failure("Kayıt bulunamadı.");
            }
            var entityDto = new AppSettingLogoDto
            {
                Id = result.Id,
                FaviconPath = result.FaviconPath,
                LogoPath = result.LogoPath,
                MobileLogoPath = result.MobileLogoPath,
                ShareImagePath = result.ShareImagePath
            };

            return ServiceResult<AppSettingLogoDto>.Success(entityDto);
        }
    }
}
