using AutoMapper;
using Economy.Application.Dtos.AppContentDtos;
using Economy.Domain.Entites.EntityAppContents.AppContents;
using Economy.Domain.Entites.EntityCategories;

namespace Economy.Application.Profiles
{
    public class AppContentProfile : Profile
    {
        public AppContentProfile()
        {
            CreateMap<AppContent, AppContentDto>()
             .ForMember(dest => dest.AppCategory, opt => opt.MapFrom(src => src.AppCategory));

            CreateMap<AppContentTranslation, AppContentTranslationDto>();
            
            CreateMap<AppCategory, AppCategoryDto>();
        }
    }
  
}
