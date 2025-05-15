using Economy.Core.Interfaces;
using Economy.Core.Tools;
using Economy.Domain.Entites.EntityAppLanguage;
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

        public PanelAppSettingLogoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _appSettingLogoRepository = unitOfWork.EntityRepository<AppSettingLogo>();
        }


        public ResponseModel<AppSettingLogoDto> CreateAppSettingLogo(AppSettingLogoCreateDto model)
        {
            throw new NotImplementedException();
        }

        public ResponseModel<AppSettingLogoDto> DeleteAppSettingLogo(int id)
        {
            throw new NotImplementedException();
        }

        public ResponseModel<AppSettingLogoDto> EditAppSettingLogo(AppSettingLogoEditDto model)
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
