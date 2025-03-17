using AutoMapper;
using Economy.Application.Dtos.AppLanguageDtos;
using Economy.Application.Dtos.AppPageDtos;
using Economy.Domain.Entites.EntityAppLanguage;
using Economy.Domain.Entites.EntityAppPages;

namespace Economy.Application.Profiles
{
    public class AppLanguageProfile : Profile
    {
        public AppLanguageProfile()
        {
            CreateMap<AppLanguage, AppLanguageDto>()
                .ForMember(dest => dest.AppSettingTranslations, opt => opt.MapFrom(src => src.AppSettingTranslations));

            CreateMap<AppPageTranslation, AppPageTranslationDto>();

            
        }
    }
   
}
