using AutoMapper;
using Economy.Application.TenantUI.Dtos.AppTechnicalSettingDtos;
using Economy.Domain.Entites.TenantEntity.EntityAppSettings;

namespace Economy.Application.TenantUI.Mappings
{

    public sealed class AppTechnicalSettingProfile : Profile
    {
        public AppTechnicalSettingProfile()
        {
            CreateMap<AppTechnicalSetting, AppTechnicalSettingDto>().ReverseMap();
            CreateMap<AppTechnicalSetting, AppTechnicalSettingCreateEditDto>().ReverseMap();
            CreateMap<AppTechnicalSettingDto, AppTechnicalSettingCreateEditDto>().ReverseMap();
        }
    }
}
