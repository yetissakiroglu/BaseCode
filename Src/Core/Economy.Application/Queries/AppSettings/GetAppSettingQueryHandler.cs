using Economy.Application.Dtos.AppMenuDtos;
using Economy.Application.Dtos.AppSettingDtos;
using Economy.Application.Interfaces;
using Economy.Application.Queries.AppMenus;
using Economy.Core.Tools;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
