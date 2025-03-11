using Economy.Application.Dtos.AppSettingDtos;
using Economy.Application.Interfaces;
using Economy.Core.Tools;
using MediatR;

namespace Economy.Application.Queries.AppSettings
{
    public class GetAppSettingQueryHandler(IAppSettingService appSettingService) : IRequestHandler<GetAppSettingQuery, ResponseModel<AppSettingDto>>
    {
        private readonly IAppSettingService _appSettingService = appSettingService;
        public async Task<ResponseModel<AppSettingDto>> Handle(GetAppSettingQuery request, CancellationToken cancellationToken)
        {
            return await _appSettingService.GetForReadAsync(request);
        }
    }
    
}
