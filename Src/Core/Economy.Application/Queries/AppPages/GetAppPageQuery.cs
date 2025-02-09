using Economy.Application.Dtos.AppPageDtos;
using MediatR;

namespace Economy.Application.Queries.AppPages
{
    public record GetAppPageQuery(int Id) : IRequest<AppPageDto>;

   
}
