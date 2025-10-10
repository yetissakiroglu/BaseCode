using AutoMapper;
using Economy.Application.TenantUI.Dtos.AppSettingDtos;
using Economy.Domain.Entites.TenantEntity.EntityAppSettings;

namespace Economy.Application.TenantUI.Mappings
{

    public sealed class AppSettingProfile : Profile
    {
        public AppSettingProfile()
        {
            CreateMap<AppSetting, AppSettingCreateEditDto>().ReverseMap();
            CreateMap<AppSetting, AppSettingDto>().ReverseMap();
            CreateMap<AppSettingCreateEditDto, AppSettingDto>().ReverseMap();
            CreateMap<AppSettingTranslation, AppSettingTranslationDto>().ReverseMap();
        }
    }
}
