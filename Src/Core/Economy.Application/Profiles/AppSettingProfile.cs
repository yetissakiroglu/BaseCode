using AutoMapper;
using Economy.Application.Dtos.AppSettingDtos;
using Economy.Domain.Entites.EntityAppSettings;

namespace Economy.Application.Profiles
{
    
    public class AppSettingProfile : Profile
    {
        public AppSettingProfile()
        {
            CreateMap<AppSetting, AppSettingDto>()
                .ForMember(dest => dest.Translations, opt => opt.MapFrom(src => src.Translations));

            CreateMap<AppSettingTranslation, AppSettingTranslationDto>();
        }
    }

}
