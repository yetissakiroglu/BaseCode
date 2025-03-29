using AutoMapper;
using Economy.Application.Dtos.AppLanguageDtos;
using Economy.Domain.Entites.EntityAppLanguage;

namespace Economy.Application.Profiles
{
    public class AppLanguageProfile : Profile
    {
        public AppLanguageProfile()
        {
            CreateMap<AppLanguage, AppLanguageDto>();


            
        }
    }
   
}
