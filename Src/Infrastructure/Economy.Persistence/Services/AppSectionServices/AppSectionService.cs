using Economy.Application.ApiDtos;
using Economy.Application.Repositories.AppSectionRepositories;
using Economy.Application.Services.AppSectionServices;
using Economy.Application.UnitOfWorks;
using Economy.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Economy.Persistence.Services.AppSectionServices
{
    public class AppSectionService(IAppSectionRepository repository, IUnitOfWork unitOfWork)
        : IAppSectionService
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
