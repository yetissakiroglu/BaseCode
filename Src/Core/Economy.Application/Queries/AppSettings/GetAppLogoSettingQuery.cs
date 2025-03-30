using Economy.Application.Dtos.AppSettingDtos;
using Economy.Core.Tools;
using MediatR;

namespace Economy.Application.Queries.AppSettings
{
    public record GetAppLogoSettingQuery() : IRequest<ResponseModel<AppLogoSettingDto>>;

}
