using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Economy.Application.Dtos.AppTechnicalSettingDtos;
using Economy.Domain.Entites.EntityAppSettings;

namespace Economy.Application.Mapping
{

    public sealed class AppTechnicalSettingProfile : Profile
    {
        public AppTechnicalSettingProfile()
        {
            CreateMap<AppTechnicalSetting, AppTechnicalSettingDto>().ReverseMap();
            CreateMap<AppTechnicalSetting, AppTechnicalSettingCreateEditDto>().ReverseMap();
        }
    }
}
