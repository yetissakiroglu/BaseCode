using AutoMapper;
using Economy.Application.TenantUI.Dtos;
using Economy.Domain.Entites.TenantEntity.EntityAppBlocks;

namespace Economy.Application.TenantUI.Mappings
{
    public class AppBlockMappingProfile : Profile
    {
        public AppBlockMappingProfile()
        {
            CreateMap<AppBlockGroup, BlockGroupDto>().ReverseMap();
            CreateMap<BlockGroupDto, AppBlockGroup>().ReverseMap();
            CreateMap<BlockGroupTranslationDto, AppBlockGroupTranslation>().ReverseMap();
        }
    }

}
