using Economy.Application.Dtos.AppDtos;
using Economy.Application.Validations.AppValidator;
using Economy.Core.Tools;
using Economy.Core.Tools.Result;
using Economy.Domain.Entites.AppEntities;
using Economy.Domain.Entites.EntityAppLanguage;
using Economy.Panel.Application.Dtos.AppDtos;
using Economy.Panel.Application.Dtos.AppLanguageDtos;
using Economy.Panel.Application.Extensions;
using Economy.Panel.Application.Interfaces;
using System.Net;

namespace Economy.Core.Interfaces.Economy.Panel.Persistence.Services
{
    public class PanelAppService : IPanelAppService
    {
        private readonly IEntityRepository<App, int> _panelAppRepository;
        private readonly IUnitOfWork _unitOfWork;
        public PanelAppService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _panelAppRepository = unitOfWork.DefaultEntityRepository<App>();
        }
        public async Task<ServiceResult<AppDto>> GetAppById(int id)
        {
            var entity = _panelAppRepository.GetForRead(x => x.Id == id);
            if (entity == null)
            {
                return ServiceResult<AppDto>.Failure(
                    message: $"ID'si {id} olan uygulama bulunamadı.",
                    statusCode: (int)HttpStatusCode.NotFound
                );
            }

            var dto = new AppDto
            {
                Id = entity.Id,
                HotelName = entity.HotelName,
                ServerName = entity.ServerName,
                DatabaseName = entity.DatabaseName,
                UserName = entity.UserName,
                IsPassword = entity.IsPassword,
                Password = entity.Password,
                Domain = entity.Domain
            };

            return ServiceResult<AppDto>.Success(
                dto,
                message: "Uygulama başarıyla getirildi.",
                statusCode: (int)HttpStatusCode.OK
            );
        }

        public ResponseModel<IEnumerable<AppDto>> Apps(bool isDeleted)
        {
            var result = _panelAppRepository.WhereForRead(x => x.IsDeleted == isDeleted).Select(x => new AppDto
            {
                Id = x.Id,
                HotelName = x.HotelName,
                ServerName = x.ServerName,
                DatabaseName = x.DatabaseName,
                UserName = x.UserName,
                IsPassword = x.IsPassword,
                Password = x.Password,
                Domain = x.Domain,
            }).ToList();

            return ResponseModel<IEnumerable<AppDto>>.Success(result, System.Net.HttpStatusCode.OK);
        }

        public async Task<ServiceResult<AppDto>> CreateApp(AppCreateEditDto model)
        {
            var validator = new AppCreateEditDtoValidator();
            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                return ServiceResult<AppDto>.Failure(
                           message: "Geçersiz giriş verisi.",
                           statusCode: (int)HttpStatusCode.BadRequest,
                           validationErrors: validationResult.ToValidationDictionary());
            }

            var entity = new App
            {
                HotelName = model.HotelName,
                ServerName = model.ServerName,
                DatabaseName = model.DatabaseName,
                UserName = model.UserName,
                IsPassword = model.IsPassword,
                Password = model.Password,
                Domain = model.Domain,
            };

            _panelAppRepository.Add(entity);
            _unitOfWork.SaveDefaultChanges();

            var dto = new AppDto
            {
                Id = entity.Id,
                HotelName = entity.HotelName,
                ServerName = entity.ServerName,
                DatabaseName = entity.DatabaseName,
                UserName = entity.UserName,
                IsPassword = entity.IsPassword,
                Password = entity.Password,
                Domain = entity.Domain
            };

            return ServiceResult<AppDto>.Success(
                dto,
                message: "Başarıyla oluşturuldu.",
                statusCode: (int)HttpStatusCode.Created
            );
        }

        public ResponseModel<AppDto> DeleteApp(int Id)
        {
            var result = _panelAppRepository.GetForEdit(x => x.Id == Id);
            if (result is null)
            { return ResponseModel<AppDto>.Fail("App not found", System.Net.HttpStatusCode.NotFound); }
            result.IsDeleted = true;
            _panelAppRepository.Update(result);
            _unitOfWork.SaveDefaultChanges();
            return ResponseModel<AppDto>.Success(new AppDto
            {
                Id = result.Id,
                HotelName = result.HotelName,
                ServerName = result.ServerName,
                DatabaseName = result.DatabaseName,
                UserName = result.UserName,
                IsPassword = result.IsPassword,
                Password = result.Password,
                Domain = result.Domain,
            }, System.Net.HttpStatusCode.OK);
        }

        public Task<ResponseModel<AppDto>> EditApp(AppEditDto user)
        {
            var result = _panelAppRepository.GetForEdit(x => x.Id == user.Id);
            if (result == null)
            {
                return Task.FromResult(ResponseModel<AppDto>.Fail("App not found", System.Net.HttpStatusCode.NotFound));
            }
            result.HotelName = user.HotelName;
            result.ServerName = user.ServerName;
            result.DatabaseName = user.DatabaseName;
            result.UserName = user.UserName;
            result.IsPassword = user.IsPassword;
            result.Password = user.Password;
            result.Domain = user.Domain;
            _panelAppRepository.Update(result);
            _unitOfWork.SaveDefaultChanges();
            var appDto = new AppDto
            {
                Id = result.Id,
                HotelName = result.HotelName,
                ServerName = result.ServerName,
                DatabaseName = result.DatabaseName,
                UserName = result.UserName,
                IsPassword = result.IsPassword,
                Password = result.Password,
                Domain = result.Domain,
            };
            return Task.FromResult(ResponseModel<AppDto>.Success(appDto, System.Net.HttpStatusCode.OK)); ;

        }

        public ResponseModel<AppDto> GetApp(int Id, bool isDeleted)
        {
            var result = _panelAppRepository.GetForRead(x => x.Id == Id && x.IsDeleted == isDeleted);
            if (result == null)
            {
                return ResponseModel<AppDto>.Fail("App not found", System.Net.HttpStatusCode.NotFound);
            }
            var appDto = new AppDto
            {
                Id = result.Id,
                HotelName = result.HotelName,
                ServerName = result.ServerName,
                DatabaseName = result.DatabaseName,
                UserName = result.UserName,
                IsPassword = result.IsPassword,
                Password = result.Password,
                Domain = result.Domain,
            };
            return ResponseModel<AppDto>.Success(appDto, System.Net.HttpStatusCode.OK);
        }

    }
}
