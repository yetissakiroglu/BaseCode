using AutoMapper;
using Economy.Application.Dtos.AppPageDtos;
using Economy.Domain.Entites.EntityAppPages;

namespace Economy.Application.Profiles
{
    public class AppPageProfile : Profile
    {
        public AppPageProfile()
        {
            CreateMap<AppPage, AppPageDto>()
                .ForMember(dest => dest.Translations, opt => opt.MapFrom(src => src.Translations));

        }
    }
  
}
