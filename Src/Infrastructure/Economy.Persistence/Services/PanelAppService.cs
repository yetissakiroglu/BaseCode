using Economy.Core.Tools;
using Economy.Domain.Entites.AppEntities;
using Economy.Panel.Application.Dtos.AppDtos;
using Economy.Panel.Application.Interfaces;

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

        public async Task<ResponseModel<AppDto>> CreateApp(AppCreateDto createApp)
        {
            var app = new App
            {
                Id = createApp.Id,
                HotelName = createApp.HotelName,
                ServerName = createApp.ServerName,
                DatabaseName = createApp.DatabaseName,
                UserName = createApp.UserName,
                IsPassword = createApp.IsPassword,
                Password = createApp.Password,
                Domain = createApp.Domain,
            };

            _panelAppRepository.Add(app);
            await _unitOfWork.SaveDefaultChangesAsync();
            return ResponseModel<AppDto>.Success(new AppDto
            {
                Id = app.Id,
                HotelName = app.HotelName,
                ServerName = app.ServerName,
                DatabaseName = app.DatabaseName,
                UserName = app.UserName,
                IsPassword = app.IsPassword,
                Password = app.Password,
                Domain = app.Domain,
            }, System.Net.HttpStatusCode.Created);
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
