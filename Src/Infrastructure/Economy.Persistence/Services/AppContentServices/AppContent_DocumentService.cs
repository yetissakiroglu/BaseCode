using Economy.Application.Repositories;
using Economy.Application.Repositories.AppContentRepositories;
using Economy.Application.Services.AppContentServices;
using Economy.Application.UnitOfWorks;
using Economy.Domain.Entites.EntityAppContents;
using Economy.Infrastructure.DateFormats;
using Economy.Persistence.Contexts;
using Economy.Persistence.Services.BaseServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Economy.Persistence.Services.AppContentServices
{
    public class AppContent_DocumentService(IAppContent_DocumentRepository repository, IUnitOfWork unitOfWork, IOptions<DateFormatConfiguration> dateFormatConfiguration,
        IHttpContextAccessor httpContextAccessor, IAuditColumnTransformer auditColumnTransformer, AppDbContext appContext)
        : Service<AppContent_Document, int>(repository, unitOfWork, dateFormatConfiguration, httpContextAccessor, auditColumnTransformer, appContext), IAppContent_DocumentService
    {



    }
}
