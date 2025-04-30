using AutoMapper;
using Economy.Application.Dtos.AppPageDtos;
using Economy.Application.Interfaces;
using Economy.Application.Queries.AppPages;
using Economy.Application.Repositories.AppPageRepositories;
using Economy.Core.Interfaces;
using Economy.Core.Tools;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Economy.Persistence.Services
{

    public class AppPageService(IAppPageRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
    : IAppPageService
    {
        private readonly IAppPageRepository _appPageRepository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public ResponseModel<AppPageDto> GetForReadDefaultPage(GetAppPageDefaultQuery query)
        {

            var appModel = _appPageRepository.GetForRead(x => x.IsHomePage, x => x.Translations);
            // Eğer data bulunamazsa, hata döndürüyoruz
            if (appModel == null)
            {
                return ResponseModel<AppPageDto>.Fail("kayıt bulunamadı", HttpStatusCode.NotFound);
            }

            var appSettingDto = _mapper.Map<AppPageDto>(appModel);
            return ResponseModel<AppPageDto>.Success(appSettingDto, HttpStatusCode.OK);

        }

        public ResponseModel<AppPageDto> GetForReadPageByLanguageCodeByUrl(GetAppPageByLanguageCodeByUrlQuery query)
        {
            var appModel = _appPageRepository.GetForReadFunc(null, q => q.Include(x => x.Translations.Where(w => w.AppLanguage.Code == query.LanguageCode && w.Url == query.Url))
                                                                         .Include(x => x.AppPageSections)
                                                                         .ThenInclude(ps => ps.AppSection)
                                                                         .ThenInclude(s => s.AppSectionImages));

            var appModelDto = _mapper.Map<AppPageDto>(appModel);
            //appModelDto.Breadcrumb = appModel.GetBreadcrumbs();
            //appModelDto.AppPageSections = appModelDto.AppPageSections.OrderBy(e => e.Sequence).ToList();
            return ResponseModel<AppPageDto>.Success(appModelDto, HttpStatusCode.OK);
        }

        public ResponseModel<AppPageDto> GetForReadPageDefaultByLanguageCode(GetAppPageDefaultByLanguageCodeQuery query)
        {
            var appModel = _appPageRepository.GetForReadFunc(null, q => q.Include(x => x.Translations.Where(w => w.AppLanguage.Code == query.LanguageCode))
                                                                         .Include(x => x.AppPageSections)
                                                                         .ThenInclude(ps => ps.AppSection)
                                                                         .ThenInclude(s=>s.AppSectionImages));

            var appModelDto = _mapper.Map<AppPageDto>(appModel);
            //appModelDto.Breadcrumb = appModel?.GetBreadcrumbs();
            //appModelDto.AppPageSections = appModelDto.AppPageSections.OrderBy(e => e.Sequence).ToList();
            return ResponseModel<AppPageDto>.Success(appModelDto, HttpStatusCode.OK);
        }


    }


}
