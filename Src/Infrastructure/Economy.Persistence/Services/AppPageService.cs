using AutoMapper;
using Economy.Application.Dtos.AppPageDtos;
using Economy.Application.Interfaces;
using Economy.Application.Queries.AppPages;
using Economy.Application.Repositories.AppPageRepositories;
using Economy.Core.Tools;
using Economy.Core.UnitOfWorks;
using System.Net;

namespace Economy.Persistence.Services
{

    public class AppPageService(IAppPageRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
    : IAppPageService
    {
        private readonly IAppPageRepository _appPageRepository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseModel<AppPageDto>> GetForReadDefaultPageAsync(GetAppPageDefaultQuery query)
        {

            var appModel = await _appPageRepository.GetForReadAsync(x => x.IsHomePage, x => x.Translations);
            // Eğer data bulunamazsa, hata döndürüyoruz
            if (appModel == null)
            {
                return ResponseModel<AppPageDto>.Fail("kayıt bulunamadı", HttpStatusCode.NotFound);
            }

            var appSettingDto = _mapper.Map<AppPageDto>(appModel);
            return ResponseModel<AppPageDto>.Success(appSettingDto, HttpStatusCode.OK);

        }

        public Task<ResponseModel<AppPageDto>> GetForReadPageByLanguageCodeByUrlAsync(GetAppPageByLanguageCodeByUrlQuery query)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel<AppPageDto>> GetForReadPageDefaultByLanguageCodeAsync(GetAppPageDefaultByLanguageCodeQuery query)
        {
            var appModel = await _appPageRepository.GetForReadAsync(null, x => x.Translations.Where(w => w.AppLanguage.Code == query.LanguageCode),x=>x.AppPageSections);
            var appModelDto = _mapper.Map<AppPageDto>(appModel);
            return ResponseModel<AppPageDto>.Success(appModelDto, HttpStatusCode.OK);
        }


    }


}
