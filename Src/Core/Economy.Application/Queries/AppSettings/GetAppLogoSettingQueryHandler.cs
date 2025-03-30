using Economy.Application.Dtos.AppSettingDtos;
using Economy.Application.Interfaces;
using Economy.Core.Tools;
using MediatR;

namespace Economy.Application.Queries.AppSettings
{
    public class GetAppLogoSettingQueryHandler(IAppSettingService appSettingService) : IRequestHandler<GetAppLogoSettingQuery, ResponseModel<AppLogoSettingDto>>
    {
        private readonly IAppSettingService _appSettingService = appSettingService;
        public async Task<ResponseModel<AppLogoSettingDto>> Handle(GetAppLogoSettingQuery request, CancellationToken cancellationToken)
        {
            return await _appSettingService.GetForReadLogoAsync(request);
        }
    }
}
