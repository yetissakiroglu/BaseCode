using Economy.Application.ApiDtos;
using Economy.Application.Repositories;
using Economy.Application.Repositories.AppSectionRepositories;
using Economy.Application.Services.AppSectionServices;
using Economy.Application.UnitOfWorks;
using Economy.Domain.Entites.EntityAppPages;
using Economy.Infrastructure.DateFormats;
using Economy.Persistence.Contexts;
using Economy.Persistence.Repositories;
using Economy.Persistence.Services.BaseServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net;
using System.Security.Policy;

namespace Economy.Persistence.Services.AppSectionServices
{
    public class AppSectionService(IAppSectionRepository repository, IUnitOfWork unitOfWork, IOptions<DateFormatConfiguration> dateFormatConfiguration,
        IHttpContextAccessor httpContextAccessor, IAuditColumnTransformer auditColumnTransformer, AppDbContext appContext)
        : Service<AppSection, int>(repository, unitOfWork, dateFormatConfiguration, httpContextAccessor, auditColumnTransformer, appContext), IAppSectionService
    {
        public async Task<ServiceResult<ResponseSectionApiDto>> AppSectionAsync(int sectionId)
        {
            var appPage = await repository.Table
                .ApplyIsDeletedFalseFilter(true)
                         .Where(pt => pt.Id == sectionId)
                                 .FirstOrDefaultAsync();

            // Eğer bir kayıt bulunmadıysa hata döndürüyoruz
            if (appPage == null)
            {
                return ServiceResult<ResponseSectionApiDto>.Fail("Home page not found.", HttpStatusCode.NotFound);
            }
            // AppPage entity'sini DTO'ya dönüştürüyoruz
            var appPageDto = ResponseSectionApiDto.FromEntity(appPage);

            return ServiceResult<ResponseSectionApiDto>.Success(appPageDto, HttpStatusCode.OK);
        }
    }
}
