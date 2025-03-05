using Economy.Application.Dtos.AppLanguageDtos;
using Economy.Application.Interfaces;
using Economy.Core.Tools;
using MediatR;

namespace Economy.Application.Queries.AppLanguages
{
    public class GetAllAppLanguageQueryHandler(IAppLanguageService appLanguageService) : IRequestHandler<GetAllAppLanguageQuery, ResponseModel<List<AppLanguageDto>>>
    {
        private readonly IAppLanguageService _appLanguageService = appLanguageService;
        public async Task<ResponseModel<List<AppLanguageDto>>> Handle(GetAllAppLanguageQuery request, CancellationToken cancellationToken)
        {
            return await _appLanguageService.GetAllForReadAsync(request);
        }
    }
   
}
