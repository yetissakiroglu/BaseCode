using Economy.Application.Dtos.AppMenuDtos;
using Economy.Core.Tools;
using MediatR;

namespace Economy.Application.Queries.AppMenus
{
    public record GetAppMenuByMenuIdQuery(int MenuId) : IRequest<ResponseModel<AppMenuDto>>;    
}
