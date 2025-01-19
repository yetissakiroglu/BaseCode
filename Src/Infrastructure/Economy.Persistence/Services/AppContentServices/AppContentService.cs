using Economy.Application.ApiDtos;
using Economy.Application.Repositories.AppContentRepositories;
using Economy.Application.Services.AppContentServices;
using Economy.Application.UnitOfWorks;
using Economy.Domain.Enums;
using Economy.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Economy.Persistence.Services.AppContentServices
{
    public class AppContentService(IAppContentRepository repository, IUnitOfWork unitOfWork)
        :  IAppContentService
    {
        private readonly IAppContentRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ServiceResult<List<ResponseHeadlineApiDto>>> AppContentHeadlineListAsync(int countRow)
        {
            var appContents = await _repository.Table
                .ApplyIsDeletedFalseFilter(true)
                .Where(ac => ac.ContentType == ContentType.News && ac.IsHeadline && ac.PublicationStatus == PublicationStatus.Published && ac.ApprovalStatus == ApprovalStatus.Approved)
                .OrderByDescending(ac => ac.Id)
                .Take(countRow)
                   .Include(h => h.AppCategory)
                    .ThenInclude(c => c.ParentCategory)
                .ToListAsync();

            var appContentDtos = ResponseHeadlineApiDto.FromEntities(appContents);

            if (appContentDtos == null || appContentDtos.Count == 0)
            {
                return ServiceResult<List<ResponseHeadlineApiDto>>.Fail("Bulunamadı.", HttpStatusCode.NotFound);
            }

            return ServiceResult<List<ResponseHeadlineApiDto>>.Success(appContentDtos, HttpStatusCode.OK);
        }

        public async Task<ServiceResult<List<ResponseFeaturedApiDto>>> AppContentFeaturedListAsync(int countRow)
        {
            var appContents = await _repository.Table
                .ApplyIsDeletedFalseFilter(true)
                .Where(ac => ac.ContentType == ContentType.News && ac.IsFeatured && ac.PublicationStatus == PublicationStatus.Published && ac.ApprovalStatus == ApprovalStatus.Approved)
                .OrderByDescending(ac => ac.Id)
                .Take(countRow)
                   .Include(h => h.AppCategory)
                    .ThenInclude(c => c.ParentCategory)
                .ToListAsync();

            var appContentDtos = ResponseFeaturedApiDto.FromEntities(appContents);

            if (appContentDtos == null || appContentDtos.Count == 0)
            {
                return ServiceResult<List<ResponseFeaturedApiDto>>.Fail("Bulunamadı.", HttpStatusCode.NotFound);
            }

            return ServiceResult<List<ResponseFeaturedApiDto>>.Success(appContentDtos, HttpStatusCode.OK);
        }

        public async Task<ServiceResult<List<ResponseBreakingNewsApiDto>>> AppContentBreakingNewsListAsync(int countRow)
        {
            var appContents = await _repository.Table
                .ApplyIsDeletedFalseFilter(true)
                .Where(ac => ac.ContentType == ContentType.News && ac.IsBreakingNews && ac.PublicationStatus == PublicationStatus.Published && ac.ApprovalStatus == ApprovalStatus.Approved)
                .OrderByDescending(ac => ac.Id)
                .Take(countRow)
                   .Include(h => h.AppCategory)
                    .ThenInclude(c => c.ParentCategory)
                .ToListAsync();

            var appContentDtos = ResponseBreakingNewsApiDto.FromEntities(appContents);

            if (appContentDtos == null || appContentDtos.Count == 0)
            {
                return ServiceResult<List<ResponseBreakingNewsApiDto>>.Fail("Bulunamadı.", HttpStatusCode.NotFound);
            }

            return ServiceResult<List<ResponseBreakingNewsApiDto>>.Success(appContentDtos, HttpStatusCode.OK);
        }

        public async Task<ServiceResult<ResponseContentDetailApiDto>> AppContentDetailAsync(int appContentId)
        {
            var appContent = await _repository.Table
               .ApplyIsDeletedFalseFilter(true)
               .Where(ac => ac.ContentType == ContentType.News && ac.IsBreakingNews && ac.PublicationStatus == PublicationStatus.Published && ac.ApprovalStatus == ApprovalStatus.Approved && ac.Id == appContentId)
                  .Include(h => h.AppCategory)
                   .ThenInclude(c => c.ParentCategory)
                  .Include(h=>h.AppContent_ImportantNotes)
                  .Include(h=>h.AppContent_Documents)
               .FirstOrDefaultAsync();

            if (appContent is null)
            {
                return ServiceResult<ResponseContentDetailApiDto>.Fail("Bulunamadı.", HttpStatusCode.NotFound);
            }

            var appContentDto = ResponseContentDetailApiDto.FromEntity(appContent);
            return ServiceResult<ResponseContentDetailApiDto>.Success(appContentDto, HttpStatusCode.OK);
        }









        //public async  Task<ServiceResult<List<AppContentDto>>> AppContentHeadlineListAsync( int countRow)
        //{


        //    var appContents = await _repository.Table
        //        .ApplyIsDeletedFalseFilter(true)
        //        .Where(ac => ac.ContentType == ContentType.News && ac.IsHeadline) 
        //        .OrderByDescending(ac => ac.CreatedAtUtc)  
        //        .Take(countRow)                            
        //           .Include(h => h.AppCategory)
        //            .ThenInclude(c => c.ParentCategory)
        //        .ToListAsync();

        //    // Assuming FromEntity() exists for AppContentDto as well
        //    var appContentDtos = appContents
        //      .Select(ac => AppContentDto.FromEntity(ac))
        //      .ToList();

        //    if (appContentDtos == null || appContentDtos.Count == 0) // Checking if the list is null or empty
        //    {
        //        return ServiceResult<List<AppContentDto>>.Fail("Bulunamadı.", HttpStatusCode.NotFound);
        //    }

        //    return ServiceResult<List<AppContentDto>>.Success(appContentDtos, HttpStatusCode.OK);
        //}

        //public async Task<ServiceResult<List<ResponseFeaturedApiDto>>> AppContentIsFeaturedListAsync(ContentType contentType, int countRow, bool IsFeatured)
        //{
        //    var appContents = await _repository.Table
        //        .ApplyIsDeletedFalseFilter(true)
        //        .Where(ac => ac.ContentType == contentType && ac.IsHeadline == IsFeatured)  // Filter by contentType
        //        .OrderByDescending(ac => ac.CreatedAtUtc)    // Assuming CreatedDate exists for ordering by the latest
        //        .Take(countRow)                             // Limit to countRow number of records
        //           .Include(h => h.AppCategory)
        //            .ThenInclude(c => c.ParentCategory)
        //        .ToListAsync();

        //    // Assuming FromEntity() exists for AppContentDto as well
        //    var appContentDtos = ResponseFeaturedApiDto.FromEntities(appContents);


        //    if (appContentDtos == null || appContentDtos.Count == 0) // Checking if the list is null or empty
        //    {
        //        return ServiceResult<List<ResponseFeaturedApiDto>>.Fail("Bulunamadı.", HttpStatusCode.NotFound);
        //    }

        //    return ServiceResult<List<ResponseFeaturedApiDto>>.Success(appContentDtos, HttpStatusCode.OK);
        //}


        //public async Task<ServiceResult<List<AppContentDto>>> AppContentIsIsBreakingNewsListAsync(ContentType contentType, int countRow, bool IsBreakingNews)
        //{
        //    var appContents = await _repository.Table
        //       .ApplyIsDeletedFalseFilter(true)
        //       .Where(ac => ac.ContentType == contentType && ac.IsHeadline == IsBreakingNews)  // Filter by contentType
        //       .OrderByDescending(ac => ac.CreatedAtUtc)    // Assuming CreatedDate exists for ordering by the latest
        //       .Take(countRow)                             // Limit to countRow number of records
        //          .Include(h => h.AppCategory)
        //           .ThenInclude(c => c.ParentCategory)
        //       .ToListAsync();

        //    // Assuming FromEntity() exists for AppContentDto as well
        //    var appContentDtos = appContents
        //      .Select(ac => AppContentDto.FromEntity(ac))
        //      .ToList();

        //    if (appContentDtos == null || appContentDtos.Count == 0) // Checking if the list is null or empty
        //    {
        //        return ServiceResult<List<AppContentDto>>.Fail("Bulunamadı.", HttpStatusCode.NotFound);
        //    }

        //    return ServiceResult<List<AppContentDto>>.Success(appContentDtos, HttpStatusCode.OK);
        //}













        //public async Task<ServiceResult<AppContentDto>> AppContentAsync(int id)
        //{
        //    var hasResult = await _entityRepository.Table.Where(a => a.Id == id) // Ana tabloda dil filtresi uyguluyoruz
        //                  .Include(w => w.AppImageGroups) // AppImageGroups ilişkili tabloyu dahil ediyoruz
        //                      .ThenInclude(w => w.AppImages) // AppImageGroups üzerinden AppImages ilişkisini dahil ediyoruz
        //                  .Include(w => w.AppAmenityGroups) // AppAmenityGroups ilişkili tabloyu dahil ediyoruz
        //                      .ThenInclude(w => w.AppAmenities) // AppAmenityGroups üzerinden AppAmenities ilişkisini dahil ediyoruz
        //                  .FirstOrDefaultAsync();


        //    if (hasResult is null)
        //    {
        //        return ServiceResult<AppContentDto>.Fail("ürün bulunamadı", HttpStatusCode.NotFound);
        //    }

        //    var appPageDto = AppContentDto.FromEntity(hasResult, _languageProvider.CurrentLanguage);

        //    return ServiceResult<AppContentDto>.Success(appPageDto, HttpStatusCode.OK);
        //}

        //public async Task<ServiceResult<AppContentDto>> AppContentAsync(string url, int id)
        //{
        //    var hasResult = await _entityRepository.Table.Where(w => w.Slug == url && w.Id == id)
        //       .Include(w => w.AppImageGroups)
        //       .ThenInclude(w => w.AppImages)
        //       .Include(w => w.AppAmenityGroups)
        //       .ThenInclude(w => w.AppAmenities).FirstOrDefaultAsync();

        //    if (hasResult is null)
        //    {
        //        return ServiceResult<AppContentDto>.Fail("ürün bulunamadı", HttpStatusCode.NotFound);
        //    }

        //    var appPageDto = AppContentDto.FromEntity(hasResult, _languageProvider.CurrentLanguage);

        //    return ServiceResult<AppContentDto>.Success(appPageDto, HttpStatusCode.OK);
        //}

        //public async Task<ServiceResult<List<AppContentDto>>> AppContents()
        //{
        //    // Veritabanından ilgili sayfaları çekiyoruz
        //    var hasResult = await _entityRepository.Table
        //         .Include(w => w.AppImageGroups)
        //       .ThenInclude(w => w.AppImages)
        //       .Include(w => w.AppAmenityGroups)
        //       .ThenInclude(w => w.AppAmenities)
        //        .ToListAsync();  // Asenkron olarak listeye çevirme


        //    if (hasResult is null)
        //    {
        //        return ServiceResult<List<AppContentDto>>.Fail("ürün bulunamadı", HttpStatusCode.NotFound);
        //    }

        //    var appPageDto = AppContentDto.List(hasResult, _languageProvider.CurrentLanguage);

        //    return ServiceResult<List<AppContentDto>>.Success(appPageDto, HttpStatusCode.OK);
        //}

        //public async Task<ServiceResult<List<AppContentDto>>> AppContentsAsync(List<int> ids)
        //{
        //    // Veritabanından ilgili sayfaları çekiyoruz
        //    var hasResult = await _entityRepository.Table.Where(w => ids.Contains(w.Id))
        //         .Include(w => w.AppImageGroups)
        //       .ThenInclude(w => w.AppImages)
        //       .Include(w => w.AppAmenityGroups)
        //       .ThenInclude(w => w.AppAmenities)
        //        .ToListAsync();  // Asenkron olarak listeye çevirme


        //    if (hasResult is null)
        //    {
        //        return ServiceResult<List<AppContentDto>>.Fail("ürün bulunamadı", HttpStatusCode.NotFound);
        //    }

        //    var appPageDto = AppContentDto.List(hasResult, _languageProvider.CurrentLanguage);

        //    return ServiceResult<List<AppContentDto>>.Success(appPageDto, HttpStatusCode.OK);
        //}



    }
}
