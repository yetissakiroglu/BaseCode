using Economy.Application.Dtos.AppLanguageDtos;
using Economy.Core.Tools;
using MediatR;

namespace Economy.Application.Queries.AppLanguages
{
    public record GetAllAppLanguageQuery(bool IsActive) : IRequest<ResponseModel<List<AppLanguageDto>>>;

   
}
