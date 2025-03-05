using Economy.Application.Dtos.AppLanguageDtos;
using Economy.Application.Interfaces;
using Economy.Core.Tools;
using MediatR;

namespace Economy.Application.Queries.AppLanguages
{
    public class GetAppLanguageByDefaultQueryHandler(IAppLanguageService appLanguageService) : IRequestHandler<GetAppLanguageByDefaultQuery, ResponseModel<AppLanguageDto>>
    {
        private readonly IAppLanguageService _appLanguageService = appLanguageService;
        public async Task<ResponseModel<AppLanguageDto>> Handle(GetAppLanguageByDefaultQuery request, CancellationToken cancellationToken)
        {
            return await _appLanguageService.GetDefaultForReadAsync(request);
        }
    }
   
}
