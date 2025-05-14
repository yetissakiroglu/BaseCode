using Economy.Core.Tools;
using Economy.Domain.Entites.EntityAppLanguage;
using Economy.Panel.Application.Dtos.AppLanguageDtos;
using Economy.Panel.Application.Interfaces;
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

        public ResponseModel<IEnumerable<AppLanguageDto>> GetAllLanguage(bool isDeleted, bool isActive)
        {
            var result = _appLanguageRepository.WhereForRead(w => w.IsDeleted == isDeleted && w.IsActive == isActive);

            if (!result.Any())
                return ResponseModel<IEnumerable<AppLanguageDto>>.Success(HttpStatusCode.OK);

            var dtoList = result.Select(x => new AppLanguageDto
            {
                IsDefault = x.IsDefault,
                Code = x.Code,
                Icon = x.Icon,
                Id = x.Id,
                IsActive = x.IsActive,
                IsRTL = x.IsRTL,
                Name = x.Name,
            }).ToList();

            return ResponseModel<IEnumerable<AppLanguageDto>>.Success(dtoList, System.Net.HttpStatusCode.OK);

        }
        public ResponseModel<IEnumerable<AppLanguageDto>> GetAllLanguage(bool isDeleted)
        {
            var result = _appLanguageRepository.WhereForRead(w => w.IsDeleted == isDeleted);

            if (!result.Any())
                return ResponseModel<IEnumerable<AppLanguageDto>>.Success(HttpStatusCode.OK);

            var dtoList = result.Select(x => new AppLanguageDto
            {
                IsDefault = x.IsDefault,
                Code = x.Code,
                Icon = x.Icon,
                Id = x.Id,
                IsActive = x.IsActive,
                IsRTL = x.IsRTL,
                Name = x.Name,
            }).ToList();

            return ResponseModel<IEnumerable<AppLanguageDto>>.Success(dtoList, System.Net.HttpStatusCode.OK);
        }
        public ResponseModel<AppLanguageDto> GetLanguage(int id, bool isDeleted)
        {

            var result = _appLanguageRepository.GetForRead(w => w.IsDeleted == isDeleted && w.Id == id);
            if (result is null)
                return ResponseModel<AppLanguageDto>.Success(HttpStatusCode.OK);

            var dto = new AppLanguageDto
            {
                IsDefault = result.IsDefault,
                Code = result.Code,
                Icon = result.Icon,
                Id = result.Id,
                IsActive = result.IsActive,
                IsRTL = result.IsRTL,
                Name = result.Name,
            };

            return ResponseModel<AppLanguageDto>.Success(dto, HttpStatusCode.OK);

        }
    }
}
