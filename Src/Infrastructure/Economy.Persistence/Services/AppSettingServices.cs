using Economy.Application.Dtos.WebSettingDtos;
using Economy.Application.Repositories;
using Economy.Application.Repositories.AppSettingRepositories;
using Economy.Application.Services;
using Economy.Application.UnitOfWorks;
using Economy.Domain.Entites.EntityAppSettings;
using Economy.Infrastructure.DateFormats;
using Economy.Persistence.Contexts;
using Economy.Persistence.Services.BaseServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net;

namespace Economy.Persistence.Services
{
    public class AppSettingServices(IAppSettingRepository repository, IUnitOfWork unitOfWork, IOptions<DateFormatConfiguration> dateFormatConfiguration,
           IHttpContextAccessor httpContextAccessor, IAuditColumnTransformer auditColumnTransformer, AppDbContext appContext)
           : Service<AppSetting, int>(repository, unitOfWork, dateFormatConfiguration, httpContextAccessor, auditColumnTransformer, appContext), IAppSettingServices
    {
        private readonly IAppSettingRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        //protected readonly DateFormatConfiguration _dateFormatConfiguration = dateFormatConfiguration.Value;
        //protected readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        //protected readonly IAuditColumnTransformer _auditColumnTransformer = auditColumnTransformer;
        //protected readonly string _userId = string.Empty;
        //protected readonly string _userName = string.Empty;


        public async Task<ServiceResult<AppSettingDto>> AppSettingDefaultAsync()
        {
            try
            {
                // Varsayılan ayarları sorgulama ve çevirileri dahil etme
                var hasResult = await _repository.Table
                              //.Include(x => x.AppSettingTranslations) // Çevirileri dahil et
                              //.Include(x => x.AppSettingLogo) // Çevirileri dahil et
                              .FirstOrDefaultAsync();

                if (hasResult == null)
                {
                    return ServiceResult<AppSettingDto>.Fail("da bulunamadı.", HttpStatusCode.NotFound);
                }

                var appSettingDto = AppSettingDto.FromEntity(hasResult);

                return ServiceResult<AppSettingDto>.Success(appSettingDto, HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                return ServiceResult<AppSettingDto>.Fail(ex.Message, HttpStatusCode.NotFound);
            }
        }




    }
}
