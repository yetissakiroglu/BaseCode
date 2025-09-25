using AutoMapper;
using Economy.Application.Dtos.AppTechnicalSettingDtos;
using Economy.Application.Interfaces;
using Economy.Core.Helpers;
using Economy.Core.Interfaces;
using Economy.Core.Tools.Result;
using Economy.Domain.Entites.EntityAppSettings;
using Economy.Panel.Application.Extensions;
using FluentValidation;

namespace Economy.Persistence.Services
{
    public sealed class PanelAppTechnicalSettingService : IPanelAppTechnicalSettingService
    {

        private readonly IEntityRepository<AppTechnicalSetting, int> _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<AppTechnicalSettingCreateEditDto> _validator;
        public PanelAppTechnicalSettingService(IUnitOfWork unitOfWork, IFileImageHelperService fileImageHelperService, IValidator<AppTechnicalSettingCreateEditDto> validator, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _repo = unitOfWork.HotelEntityRepository<AppTechnicalSetting>();
            _validator = validator;
            _mapper = mapper;
        }



        public ServiceResult<AppTechnicalSettingDto> SaveAppTechnicalSetting(AppTechnicalSettingCreateEditDto appSettingDto)
        {
            var validation = _validator.Validate(appSettingDto);
            if (!validation.IsValid)
            {
                return ServiceResult<AppTechnicalSettingDto>.Failure(
                    "Doğrulama hatası",
                    validationErrors: validation.ToValidationDictionary()
                );
            }

            AppTechnicalSetting? entity;

            if (appSettingDto.Id > 0)
            {
                entity = _repo.DataSet.FirstOrDefault(x => x.Id == appSettingDto.Id);
                if (entity == null)
                    return ServiceResult<AppTechnicalSettingDto>.Failure("Kayıt bulunamadı.");

                _mapper.Map(appSettingDto, entity);
                _repo.Update(entity);
            }
            else
            {
                entity = _mapper.Map<AppTechnicalSetting>(appSettingDto);
                _repo.Add(entity);
            }

            _unitOfWork.SaveHotelChanges();

            var dto = _mapper.Map<AppTechnicalSettingDto>(entity);
            return ServiceResult<AppTechnicalSettingDto>.Success(dto, "Kayıt başarılı.");

        }

        public ServiceResult<AppTechnicalSettingDto> GetAppTechnicalSetting(bool isDeleted)
        {
            var entity = _repo.DataSet.Where(x => x.IsDeleted == isDeleted)
                                 .OrderByDescending(x => x.Id)
                                 .FirstOrDefault();

            if (entity == null)
                return ServiceResult<AppTechnicalSettingDto>.Empty("Herhangi bir kayıt bulunamadı.");

            var dto = _mapper.Map<AppTechnicalSettingDto>(entity);
            return ServiceResult<AppTechnicalSettingDto>.Success(dto);

        }

        public ServiceResult<AppTechnicalSettingDto> DeleteAppTechnicalSetting(int id)
        {

            var entity = _repo.DataSet.FirstOrDefault(x => x.Id == id);
            if (entity == null)
                return ServiceResult<AppTechnicalSettingDto>.Failure("Kayıt bulunamadı.");

            _repo.Delete(entity);
            _unitOfWork.SaveHotelChanges();

            var dto = _mapper.Map<AppTechnicalSettingDto>(entity);
            return ServiceResult<AppTechnicalSettingDto>.Success(dto, "Kayıt silindi.");
        }
    }
}


