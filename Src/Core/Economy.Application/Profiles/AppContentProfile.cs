using AutoMapper;
using Economy.Application.Dtos.AppContentDtos;
using Economy.Domain.Entites.EntityCategories;
using Economy.Domain.Entites.EntityPages;

namespace Economy.Application.Profiles
{
    public class AppContentProfile : Profile
    {
        public AppContentProfile()
        {
            CreateMap<AppContent, AppContentDto>()
             .ForMember(dest => dest.AppCategory, opt => opt.MapFrom(src => src.AppCategory));
        

           

            CreateMap<AppCategory, AppCategoryDto>();



        }
    }
  
}
