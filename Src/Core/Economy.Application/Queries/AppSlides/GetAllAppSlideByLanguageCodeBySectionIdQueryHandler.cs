using Economy.Application.Dtos.AppSlideDtos;
using Economy.Application.Interfaces;
using Economy.Core.Tools;
using MediatR;

namespace Economy.Application.Queries.AppSlides
{
    public class GetAllAppSlideByLanguageCodeBySectionIdQueryHandler(IAppSlideService appSlideService) : IRequestHandler<GetAllAppSlideByLanguageCodeBySectionIdQuery, ResponseModel<List<AppSlideDto>>>
    {
        private readonly IAppSlideService _appSlideService = appSlideService;
        public async Task<ResponseModel<List<AppSlideDto>>> Handle(GetAllAppSlideByLanguageCodeBySectionIdQuery request, CancellationToken cancellationToken)
        {
            return await _appSlideService.WhereForReadByLanguageCodeBySectionIdAsync(request);
        }
    }
  
}
