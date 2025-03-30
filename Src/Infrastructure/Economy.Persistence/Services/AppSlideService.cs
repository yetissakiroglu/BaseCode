using AutoMapper;
using Economy.Application.Dtos.AppSlideDtos;
using Economy.Application.Interfaces;
using Economy.Application.Queries.AppSlides;
using Economy.Application.Repositories.AppSlideRepositories;
using Economy.Core.Tools;
using Economy.Core.UnitOfWorks;
using System.Net;

namespace Economy.Persistence.Services
{
    public class AppSlideService(IAppSlideRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
       : IAppSlideService
    {
        private readonly IAppSlideRepository _appSlideRepository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseModel<List<AppSlideDto>>> WhereForReadByLanguageCodeAsync(GetAllAppSlideByLanguageCodeQuery query)
        {
            var appSlide = _appSlideRepository.WhereForReadAsync(null, x => x.Translations.Where(w => w.AppLanguage.Code == query.LanguageCode));
            var appSlideDto = _mapper.Map<List<AppSlideDto>>(appSlide);
            return ResponseModel<List<AppSlideDto>>.Success(appSlideDto, HttpStatusCode.OK);
        }

        public async Task<ResponseModel<List<AppSlideDto>>> WhereForReadByLanguageCodeBySectionIdAsync(GetAllAppSlideByLanguageCodeBySectionIdQuery query)
        {
            var appSlide = _appSlideRepository.WhereForReadAsync(x=>x.AppSectionId==query.AppSectionId, x => x.Translations.Where(w => w.AppLanguage.Code == query.LanguageCode));
            var appSlideDto = _mapper.Map<List<AppSlideDto>>(appSlide);
            return ResponseModel<List<AppSlideDto>>.Success(appSlideDto, HttpStatusCode.OK);
        }
    }
}
