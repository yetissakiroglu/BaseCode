using AutoMapper;
using Economy.Application.TenantUI.Dtos.AppSettingLogoDtos;
using Economy.Domain.Entites.EntityAppSettings;

namespace Economy.Application.TenantUI.Mappings
{

    public sealed class AppSettingLogoProfile : Profile
    {
        public AppSettingLogoProfile()
        {
            CreateMap<AppSettingLogo, AppSettingLogoCreateEditDto>().ReverseMap();
            CreateMap<AppSettingLogo, AppSettingLogoDto>().ReverseMap();
            CreateMap<AppSettingLogoCreateEditDto, AppSettingLogoDto>().ReverseMap();
        }
    }
}
