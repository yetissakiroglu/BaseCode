using AutoMapper;
using Economy.Application.TenantUI.Dtos;
using Economy.Domain.Entites.TenantEntity.EntityAppBlocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.TenantUI.Mappings
{
    public class AppBlockMappingProfile : Profile
    {
        public AppBlockMappingProfile()
        {
            CreateMap<BlockGroup, BlockGroupDto>()
            .ForMember(d => d.Items, opt => opt.MapFrom(s => s.Items.OrderBy(i => i.SortOrder)));


            CreateMap<BlockItem, BlockItemDto>()
            .ForMember(d => d.Gallery, opt => opt.MapFrom(s => s.Gallery.OrderBy(x => x.SortOrder).Select(x => x.ImageUrl)));


            CreateMap<BlockGroupDto, BlockGroup>()
            .ForMember(d => d.Items, opt => opt.Ignore()); // item’ları servis katmanında yönet

            CreateMap<BlockGroupTranslationDto, BlockGroupTranslation>().ReverseMap();


            CreateMap<BlockItemDto, BlockItem>()
            .ForMember(d => d.Gallery, opt => opt.Ignore());
        }
    }

}
