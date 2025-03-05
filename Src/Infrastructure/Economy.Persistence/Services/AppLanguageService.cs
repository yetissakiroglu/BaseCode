using AutoMapper;
using Economy.Application.Dtos.AppLanguageDtos;
using Economy.Application.Interfaces;
using Economy.Application.Queries.AppLanguages;
using Economy.Application.Repositories.AppLanguageRepositories;
using Economy.Core.Tools;
using Economy.Core.UnitOfWorks;
using System.Net;

namespace Economy.Persistence.Services
{
    public class AppLanguageService(IAppLanguageRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
       : IAppLanguageService
    {
        private readonly IAppLanguageRepository _appLanguageRepository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseModel<List<AppLanguageDto>>> GetAllForReadAsync(GetAllAppLanguageQuery query)
        {
            var appLanguage = await _appLanguageRepository.WhereForReadAsync(w => w.IsActive == query.IsActive, x => x.AppSettingTranslations);
            var appLanguageDto = _mapper.Map<List<AppLanguageDto>>(appLanguage);
            return ResponseModel<List<AppLanguageDto>>.Success(appLanguageDto, HttpStatusCode.OK);
        }

        public async Task<ResponseModel<AppLanguageDto>> GetDefaultForReadAsync(GetAppLanguageByDefaultQuery query)
        {
            var appLanguage = await _appLanguageRepository.GetForReadAsync(null, x => x.AppSettingTranslations);

            // Eğer data bulunamazsa, hata döndürüyoruz
            if (appLanguage == null)
            {
                return ResponseModel<AppLanguageDto>.Fail("kayıt bulunamadı", HttpStatusCode.NotFound);
            }

            var appLanguageDto = _mapper.Map<AppLanguageDto>(appLanguage);
            return ResponseModel<AppLanguageDto>.Success(appLanguageDto, HttpStatusCode.OK);
        }
    }
}
