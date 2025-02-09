using Economy.Application.Dtos.AppMenuDtos;
using MediatR;

namespace Economy.Application.Queries.AppMenus
{
    public record GetAllAppMenuQuery() : IRequest<List<AppMenuDto>>;

}
