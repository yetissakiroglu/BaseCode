using AutoMapper;
using Economy.Application.Dtos.AppSlideDtos;
using Economy.Domain.Entites.EntitySlides;

namespace Economy.Application.Profiles
{
    public class AppSlideProfile : Profile
    {
        public AppSlideProfile()
        {
            CreateMap<AppSlide, AppSlideDto>()
                .ForMember(dest => dest.Translations, opt => opt.MapFrom(src => src.Translations));
            CreateMap<AppSlideTranslation, AppSlideTranslationDto>();

        }
    }
  
}
