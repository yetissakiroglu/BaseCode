using Economy.Application.Dtos;
using MediatR;

namespace Economy.Application.Queries.AppMenus
{
    public record GetAllAppMenusQuery() : IRequest<List<AppMenuDto>>;

}
