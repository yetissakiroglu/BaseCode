using Economy.Application.ApiDtos;
using Economy.Application.Repositories.AppSectionRepositories;
using Economy.Application.Services.AppSectionServices;
using Economy.Application.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Economy.Persistence.Services.AppSectionServices
{
    public class AppPageService(IAppPageRepository repository, IUnitOfWork unitOfWork)
        :  IAppPageService
    {
        public async Task<ServiceResult<ResponsePageDetailApiDto>> AppPageDetailAsync(string url)
        {
            var appPage = await repository.Table
                         .Where(pt => pt.Slug == url)
                                  .Include(p => p.AppPageSections.Where(w=>w.IsVisible && !w.IsDeleted)) 
                              .ThenInclude(ps => ps.AppSection) 
                          .FirstOrDefaultAsync();

            // Eğer bir kayıt bulunmadıysa hata döndürüyoruz
            if (appPage == null)
            {
                return ServiceResult<ResponsePageDetailApiDto>.Fail("Home page not found.", HttpStatusCode.NotFound);
            }
            // AppPage entity'sini DTO'ya dönüştürüyoruz
            var appPageDto = ResponsePageDetailApiDto.FromEntity(appPage);

            return ServiceResult<ResponsePageDetailApiDto>.Success(appPageDto, HttpStatusCode.OK);
        }
        public async Task<ServiceResult<ResponsePageDetailApiDto>> AppPageDetailDefaultAsync()
        {
            var appPage = await repository.Table
                         .Where(pt => pt.IsHomePage)
                                  .Include(p => p.AppPageSections.Where(w => w.IsVisible && !w.IsDeleted))
                              .ThenInclude(ps => ps.AppSection)
                          .FirstOrDefaultAsync();

            // Eğer bir kayıt bulunmadıysa hata döndürüyoruz
            if (appPage == null)
            {
                return ServiceResult<ResponsePageDetailApiDto>.Fail("Home page not found.", HttpStatusCode.NotFound);
            }
            // AppPage entity'sini DTO'ya dönüştürüyoruz
            var appPageDto = ResponsePageDetailApiDto.FromEntity(appPage);

            return ServiceResult<ResponsePageDetailApiDto>.Success(appPageDto, HttpStatusCode.OK);
        }
    }
}
