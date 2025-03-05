using AutoMapper;
using Economy.Application.Dtos.AppSettingDtos;
using Economy.Application.Interfaces;
using Economy.Application.Queries.AppSettings;
using Economy.Application.Repositories.AppMenuRepositories;
using Economy.Core.Tools;
using Economy.Core.UnitOfWorks;
using System.Net;

namespace Economy.Persistence.Services
{
    public class AppSettingService(IAppSettingRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
       : IAppSettingService
    {
        private readonly IAppSettingRepository _appSettingRepository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseModel<AppSettingDto>> GetForReadAsync(GetAppSettingQuery query)
        {
            var appSetting = await _appSettingRepository.GetForReadAsync(null, x => x.Translations);

            // Eğer data bulunamazsa, hata döndürüyoruz
            if (appSetting == null)
            {
                return ResponseModel<AppSettingDto>.Fail("kayıt bulunamadı", HttpStatusCode.NotFound);
            }

            var appSettingDto = _mapper.Map<AppSettingDto>(appSetting);
            return ResponseModel<AppSettingDto>.Success(appSettingDto, HttpStatusCode.OK);
        }
    }
}
