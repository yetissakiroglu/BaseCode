using AutoMapper;
using Economy.Application.Dtos.AppTechnicalSettingDtos;
using Economy.Domain.Entites.EntityAppSettings;
using Economy.Panel.Application.Dtos.AppSettingDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.Mapping
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
