using Economy.Application.Dtos.SlideDtos;
using Economy.Application.Repositories;
using Economy.Application.Services;
using Economy.Application.UnitOfWorks;
using Economy.Domain.Entites.EntitySlides;
using Economy.Infrastructure.Providers;
using Economy.Persistence.Services.BaseServices;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Economy.Persistence.Services
{
    //public class AppSlideServices(IEntityRepository<AppSlide, int> readRepository, IUnitOfWork unitOfWork, ILanguageProvider languageProvider) : Service<AppSlide, int>(readRepository, unitOfWork), IAppSlideServices
    //{
    //    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    //    private readonly IEntityRepository<AppSlide, int> _readRepository = readRepository;
    //    private readonly ILanguageProvider _languageProvider = languageProvider;

    //    public async Task<ServiceResult<List<AppSlideDto>>> AppSlidesAsync()
    //    {
    //        // Menü öğelerini çek ve SlideDto'ya dönüştür
    //        var slideEntities = await _readRepository.Table
    //            .Include(mi => mi.AppSlideTranslations) // Geçerli dildeki öğeleri filtrele
    //            .ToListAsync();

    //        // Eğer slideEntities boşsa hata döndür
    //        if (!slideEntities.Any())
    //        {
    //            return ServiceResult<List<AppSlideDto>>.Fail("Ürün bulunamadı", HttpStatusCode.NotFound);
    //        }

    //        // SlideDto'ya dönüştür
    //        var slideDtos = slideEntities
    //            .Select(mi => AppSlideDto.FromEntity(mi, _languageProvider.CurrentLanguage))
    //            .OrderBy(x => x.Sequence)
    //            .ToList();

    //        // Başarılı sonuç döndür
    //        return ServiceResult<List<AppSlideDto>>.Success(slideDtos, HttpStatusCode.OK);
    //    }

    //    public async Task<ServiceResult<List<AppSlideDto>>> AppSlidesAsync(int appSectionId)
    //    {
    //        // Menü öğelerini çek ve SlideDto'ya dönüştür
    //        var slideEntities = await _readRepository.Table
    //            .Include(mi => mi.AppSlideTranslations)
    //            .ToListAsync();

    //        // Eğer slideEntities boşsa hata döndür
    //        if (!slideEntities.Any())
    //        {
    //            return ServiceResult<List<AppSlideDto>>.Fail("Ürün bulunamadı", HttpStatusCode.NotFound);
    //        }

    //        // SlideDto'ya dönüştür
    //        var slideDtos = slideEntities
    //            .Select(mi => AppSlideDto.FromEntity(mi, _languageProvider.CurrentLanguage))
    //            .OrderBy(x => x.Sequence)
    //            .ToList();

    //        // Başarılı sonuç döndür
    //        return ServiceResult<List<AppSlideDto>>.Success(slideDtos, HttpStatusCode.OK);
    //    }
    //}
}
