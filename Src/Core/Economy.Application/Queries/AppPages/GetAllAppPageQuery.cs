using Economy.Application.Dtos.AppMenuDtos;
using Economy.Application.Dtos.AppPageDtos;
using MediatR;

namespace Economy.Application.Queries.AppPages
{
    public record GetAllAppPageQuery() : IRequest<List<AppPageDto>>;


}
