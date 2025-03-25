using Economy.Application.Dtos.AppSlideDtos;
using Economy.Application.Interfaces;
using Economy.Core.Tools;
using MediatR;

namespace Economy.Application.Queries.AppSlides
{
    public class GetAllAppSlideByLanguageCodeQueryHandler(IAppSlideService appSlideService) : IRequestHandler<GetAllAppSlideByLanguageCodeQuery, ResponseModel<List<AppSlideDto>>>
    {
        private readonly IAppSlideService _appSlideService = appSlideService;
        public async Task<ResponseModel<List<AppSlideDto>>> Handle(GetAllAppSlideByLanguageCodeQuery request, CancellationToken cancellationToken)
        {
            return await _appSlideService.WhereForReadByLanguageCodeAsync(request);
        }
    }
  
}
