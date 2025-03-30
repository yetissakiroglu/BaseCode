using AutoMapper;
using Economy.Application.Dtos.AppContentDtos;
using Economy.Application.Dtos.AppMenuDtos;
using Economy.Application.Dtos.AppPageDtos;
using Economy.Application.Interfaces;
using Economy.Application.Queries.AppContents;
using Economy.Application.Repositories.AppContentRepositories;
using Economy.Core.Tools;
using Economy.Core.UnitOfWorks;
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

        public async Task<ResponseModel<AppContentDto>> GetForReadByLanguageCodeByAppContentIdAsync(GetAppContentByLanguageCodeByAppContentIdQuery query)
        {
            var appModel = await _appContentRepository.GetForReadFuncAsync(w => w.Id == query.AppContentId,
                q => q.Include(x => x.Translations.Where(w => w.AppLanguage.Code == query.LanguageCode))
                   .Include(x => x.AppCategory));
            var appModelDto = _mapper.Map<AppContentDto>(appModel);
            return ResponseModel<AppContentDto>.Success(appModelDto, HttpStatusCode.OK);
        }

        public async Task<ResponseModel<List<AppContentDto>>> WhereForReadByLanguageCodeByAppContentIdsAsync(GetAllAppContentByLanguageCodeByAppContentIdsQuery query)
        {
            var appModel = _appContentRepository.WhereForReadFuncAsync(w => query.AppContentIds.Contains(w.Id),
                q => q.Include(x => x.Translations.Where(w => w.AppLanguage.Code == query.LanguageCode))
                   .Include(x => x.AppCategory));
            var appModelDto = _mapper.Map<List<AppContentDto>>(appModel);
            return ResponseModel<List<AppContentDto>>.Success(appModelDto, HttpStatusCode.OK);
        }

        public async Task<ResponseModel<AppContentDto>> GetForReadByLanguageCodeByUrlAsync(GetAppContentByLanguageCodeByUrlQuery query)
        {
            var appModel = await _appContentRepository.GetForReadFuncAsync(null,
                q => q.Include(x => x.Translations.Where(w => w.AppLanguage.Code == query.LanguageCode))
                   .Include(x => x.AppCategory));
            var appModelDto = _mapper.Map<AppContentDto>(appModel);
            return ResponseModel<AppContentDto>.Success(appModelDto, HttpStatusCode.OK);
        }
    }
}
