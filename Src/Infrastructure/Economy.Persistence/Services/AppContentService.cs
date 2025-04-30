using AutoMapper;
using Economy.Application.Dtos.AppContentDtos;
using Economy.Application.Dtos.AppMenuDtos;
using Economy.Application.Dtos.AppPageDtos;
using Economy.Application.Interfaces;
using Economy.Application.Queries.AppContents;
using Economy.Application.Repositories.AppContentRepositories;
using Economy.Core.Interfaces;
using Economy.Core.Tools;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Economy.Persistence.Services
{
    public class AppContentService(IAppContentRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
       : IAppContentService
    {
        private readonly IAppContentRepository _appContentRepository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public ResponseModel<AppContentDto> GetForReadByLanguageCodeByAppContentId(GetAppContentByLanguageCodeByAppContentIdQuery query)
        {
            var appModel = _appContentRepository.GetForReadFunc(w => w.Id == query.AppContentId,
                q => q.Include(x => x.Translations.Where(w => w.AppLanguage.Code == query.LanguageCode))
                   .Include(x => x.AppCategory));
            var appModelDto = _mapper.Map<AppContentDto>(appModel);
            return ResponseModel<AppContentDto>.Success(appModelDto, HttpStatusCode.OK);
        }

        public ResponseModel<List<AppContentDto>> WhereForReadByLanguageCodeByAppContentIds(GetAllAppContentByLanguageCodeByAppContentIdsQuery query)
        {
            var appModel = _appContentRepository.WhereForReadFunc(w => query.AppContentIds.Contains(w.Id),
                q => q.Include(x => x.Translations.Where(w => w.AppLanguage.Code == query.LanguageCode))
                   .Include(x => x.AppCategory));
            var appModelDto = _mapper.Map<List<AppContentDto>>(appModel);
            return ResponseModel<List<AppContentDto>>.Success(appModelDto, HttpStatusCode.OK);
        }

        public ResponseModel<AppContentDto> GetForReadByLanguageCodeByUrl(GetAppContentByLanguageCodeByUrlQuery query)
        {
            var appModel = _appContentRepository.GetForReadFunc(null,
                q => q.Include(x => x.Translations.Where(w => w.AppLanguage.Code == query.LanguageCode))
                   .Include(x => x.AppCategory));
            var appModelDto = _mapper.Map<AppContentDto>(appModel);
            return ResponseModel<AppContentDto>.Success(appModelDto, HttpStatusCode.OK);
        }
    }
}
