using Economy.Application.Dtos.AppPageDtos;
using Economy.Core.Tools;
using MediatR;

namespace Economy.Application.Queries.AppPages
{
    public record GetAppPageQuery() : IRequest<ResponseModel<AppPageDto>>;
       
}
