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

        public ResponseModel<AppLanguageDto> CreateLanguage(AppLanguageCreateDto model)
        {
            var createModel = new AppLanguage()
            {
                Code = model.Code,
                Name = model.Name,
                IsRTL = model.IsRTL,
                Icon = model.Icon,
                IsActive = model.IsActive,
                IsDefault = model.IsDefault
            };
            _appLanguageRepository.Add(createModel);
            _unitOfWork.SaveHotelChanges();
            var dto = new AppLanguageDto
            {
                IsDefault = createModel.IsDefault,
                Code = createModel.Code,
                Icon = createModel.Icon,
                Id = createModel.Id,
                IsActive = createModel.IsActive,
                IsRTL = createModel.IsRTL,
                Name = createModel.Name,
            };

            return ResponseModel<AppLanguageDto>.Success(dto, HttpStatusCode.OK);
        }

        public ResponseModel<AppLanguageDto> DeleteLanguage(int id)
        {
            var deleteModel = _appLanguageRepository.GetForRead(w => w.Id == id);
            if (deleteModel is null)
                return ResponseModel<AppLanguageDto>.Success(HttpStatusCode.NotFound);
            deleteModel.IsDeleted = true;
            _appLanguageRepository.Update(deleteModel);
            _unitOfWork.SaveHotelChanges();
            var dto = new AppLanguageDto
            {
                IsDefault = deleteModel.IsDefault,
                Code = deleteModel.Code,
                Icon = deleteModel.Icon,
                Id = deleteModel.Id,
                IsActive = deleteModel.IsActive,
                IsRTL = deleteModel.IsRTL,
                Name = deleteModel.Name,
            };
            return ResponseModel<AppLanguageDto>.Success(dto, HttpStatusCode.OK);
        }

        public ResponseModel<AppLanguageDto> EditLanguage(AppLanguageEditDto model)
        {
            var result = _appLanguageRepository.GetForRead(w => w.Id == model.Id);
            if (result is null)
                return ResponseModel<AppLanguageDto>.Success(HttpStatusCode.NotFound);
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
