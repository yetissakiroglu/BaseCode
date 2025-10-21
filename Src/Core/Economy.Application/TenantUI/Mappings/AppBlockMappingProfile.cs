using AutoMapper;
using Economy.Application.TenantUI.Dtos;
using Economy.Domain.Entites.TenantEntity.EntityAppBlocks;

namespace Economy.Application.TenantUI.Mappings
{
    public class AppBlockMappingProfile : Profile
    {
        public AppBlockMappingProfile()
        {
            CreateMap<BlockGroup, BlockGroupDto>().ReverseMap();
            CreateMap<BlockGroupDto, BlockGroup>().ReverseMap();
            CreateMap<BlockGroupTranslationDto, BlockGroupTranslation>().ReverseMap();
        }
    }

}
