using Economy.Application.Dtos.AppContentDtos;
using Economy.Application.Interfaces;
using Economy.Core.Tools;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.Queries.AppContents
{ 
    public class GetAppContentByLanguageCodeByUrlQueryHandler(IAppContentService appContentService) : IRequestHandler<GetAppContentByLanguageCodeByUrlQuery, ResponseModel<AppContentDto>>
    {
        private readonly IAppContentService _appContentService = appContentService;
    public async Task<ResponseModel<AppContentDto>> Handle(GetAppContentByLanguageCodeByUrlQuery request, CancellationToken cancellationToken)
    {
        return _appContentService.GetForReadByLanguageCodeByUrl(request);
    }
}
  
}
