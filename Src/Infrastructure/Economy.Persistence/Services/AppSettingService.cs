using AutoMapper;
using Economy.Application.Dtos.AppSettingDtos;
using Economy.Application.Interfaces;
using Economy.Application.Queries.AppSettings;
using Economy.Application.Repositories.AppMenuRepositories;
using Economy.Application.Repositories.AppSettingRepositories;
using Economy.Core.Tools;
using Economy.Core.UnitOfWorks;
using System.Net;

namespace Economy.Persistence.Services
{
    public class AppSettingService(IAppSettingRepository appSettingRepository,IAppLogoSettingRepository appLogoSettingRepository , IUnitOfWork unitOfWork, IMapper mapper)
       : IAppSettingService
    {
        private readonly IAppSettingRepository _appSettingRepository = appSettingRepository;
        private readonly IAppLogoSettingRepository _appLogoSettingRepository = appLogoSettingRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseModel<AppSettingDto>> GetForReadAsync(GetAppSettingQuery query)
        {
            var appSetting = await _appSettingRepository.GetForReadAsync(null, x => x.Translations);
            var appSettingDto = _mapper.Map<AppSettingDto>(appSetting);
            return ResponseModel<AppSettingDto>.Success(appSettingDto, HttpStatusCode.OK);
        }

        public async Task<ResponseModel<AppLogoSettingDto>> GetForReadLogoAsync(GetAppLogoSettingQuery query)
        {
            var appSetting = await _appLogoSettingRepository.GetForReadAsync();
            var appSettingDto = _mapper.Map<AppLogoSettingDto>(appSetting);
            return ResponseModel<AppLogoSettingDto>.Success(appSettingDto, HttpStatusCode.OK);
        }
    }
}
