using AutoMapper;
using Economy.Application.Dtos.AppMenuDtos;
using Economy.Domain.Entites.EntityMenuItems;

namespace Economy.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // AppMenu'dan AppMenuDto'ya dönüşüm
            CreateMap<AppMenu, AppMenuDto>()
           .ForMember(dest => dest.SubMenus, opt => opt.MapFrom(src => src.SubMenus))
           .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Translations.FirstOrDefault().Title))
           .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Translations.FirstOrDefault().Url));
        }
    }
}
