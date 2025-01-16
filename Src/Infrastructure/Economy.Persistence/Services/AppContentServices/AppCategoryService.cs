using Economy.Application.ApiDtos;
using Economy.Application.Repositories;
using Economy.Application.Repositories.AppContentRepositories;
using Economy.Application.Services.AppContentServices;
using Economy.Application.UnitOfWorks;
using Economy.Domain.Entites.EntityCategories;
using Economy.Domain.Entites.EntityPages;
using Economy.Domain.Enums;
using Economy.Infrastructure.DateFormats;
using Economy.Persistence.Contexts;
using Economy.Persistence.Repositories;
using Economy.Persistence.Services.BaseServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net;
using System.Security.Policy;

namespace Economy.Persistence.Services.AppContentServices
{
    public class AppCategoryService(IAppCategoryRepository repository, IUnitOfWork unitOfWork, IOptions<DateFormatConfiguration> dateFormatConfiguration,
        IHttpContextAccessor httpContextAccessor, IAuditColumnTransformer auditColumnTransformer, AppDbContext appContext)
        : Service<AppCategory, int>(repository, unitOfWork, dateFormatConfiguration, httpContextAccessor, auditColumnTransformer, appContext), IAppCategoryService
    {
        private readonly IAppCategoryRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        protected readonly DateFormatConfiguration _dateFormatConfiguration = dateFormatConfiguration.Value;
        protected readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        protected readonly IAuditColumnTransformer _auditColumnTransformer = auditColumnTransformer;

        public async Task<ServiceResult<ResponseCategoryDetailApiDto>> AppCategoryDetailAsync(string url)
        {
            var mdeolResult = await _repository.Table
            .ApplyIsDeletedFalseFilter(true)
              .Where(ac => ac.Slug == url)
                 .Include(h => h.ParentCategory)
                  .ThenInclude(c => c.SubCategories)
              .FirstOrDefaultAsync();

            if (mdeolResult is null)
            {
                return ServiceResult<ResponseCategoryDetailApiDto>.Fail("Bulunamadı.", HttpStatusCode.NotFound);
            }

            var appContentDto = ResponseCategoryDetailApiDto.FromEntity(mdeolResult);
            return ServiceResult<ResponseCategoryDetailApiDto>.Success(appContentDto, HttpStatusCode.OK);
        }

        public async Task<ServiceResult<List<ResponseCategoryDetailApiDto>>> AppCategoriesAsync()
        {
            var modelResult = await _repository.Table
                .ApplyIsDeletedFalseFilter(true)
                .Include(h => h.ParentCategory)
                .ThenInclude(c => c.SubCategories)
                .ToListAsync(); // Use ToListAsync to get a list of results

            if (modelResult is null || !modelResult.Any())
            {
                return ServiceResult<List<ResponseCategoryDetailApiDto>>.Fail("Bulunamadı.", HttpStatusCode.NotFound);
            }

            // Convert the list of entities to a list of DTOs
            var appContentDtos = modelResult.Select(ResponseCategoryDetailApiDto.FromEntity).ToList();

            return ServiceResult<List<ResponseCategoryDetailApiDto>>.Success(appContentDtos, HttpStatusCode.OK);
        }



        //protected readonly string _userId = string.Empty;
        //protected readonly string _userName = string.Empty;

    }
}
