namespace Economy.Persistence.Services
{
    //public class AppPageServices(IUnitOfWork unitOfWork, IEntityRepository<AppPage, int> readRepository, ILanguageProvider languageProvider) : Service<AppPage, int>(readRepository, unitOfWork), IAppPageServices
    //{
    //    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    //    private readonly IEntityRepository<AppPage, int> _readRepository = readRepository;
    //    private readonly ILanguageProvider _languageProvider = languageProvider;

    //    public async Task<ServiceResult<AppPageDto>> AppPageAsync(string url)
    //    {
    //        var appPage = await _readRepository.Table
    //                     .Where(p => p.AppPageTranslations.Any(pt => pt.Url == url)) // Ana sorguda filtreleme
    //                       .Include(p => p.AppPageTranslations
    //                     .Where(pt => pt.Url == url))  // Dil koduna göre AppPageTranslations'ı filtreleme
    //                      .Include(p => p.AppPageSections)  // AppPageSections'ı dahil ediyoruz
    //                          .ThenInclude(ps => ps.AppPageSectionTranslations)
    //                      .Include(p => p.AppPageSections)  // AppPageSections'ı tekrar dahil edip
    //                          .ThenInclude(ps => ps.AppSection)  // AppSection'ı dahil ediyoruz
    //                              .ThenInclude(s => s.AppSectionTranslations)
    //                      .FirstOrDefaultAsync();

    //        // Eğer bir kayıt bulunmadıysa hata döndürüyoruz
    //        if (appPage == null)
    //        {
    //            return ServiceResult<AppPageDto>.Fail("Home page not found.", HttpStatusCode.NotFound);
    //        }
    //        // AppPage entity'sini DTO'ya dönüştürüyoruz
    //        var appPageDto = AppPageDto.FromEntity(appPage, _languageProvider.CurrentLanguage);

    //        return ServiceResult<AppPageDto>.Success(appPageDto, HttpStatusCode.OK);
    //    }

    //    public async Task<ServiceResult<AppPageDto>> AppPageIsHomePageAsync()
    //    {
    //        var appPage = await _readRepository.Table
    //                     .Include(p => p.AppPageTranslations)
    //                     .Include(p => p.AppPageSections)  // AppPageSections'ı dahil ediyoruz
    //                         .ThenInclude(ps => ps.AppPageSectionTranslations)
    //                     .Include(p => p.AppPageSections)  // AppPageSections'ı tekrar dahil edip
    //                         .ThenInclude(ps => ps.AppSection)  // AppSection'ı dahil ediyoruz
    //                             .ThenInclude(s => s.AppSectionTranslations)
    //                     .FirstOrDefaultAsync(p => p.IsHomePage);

    //        // Eğer bir kayıt bulunmadıysa hata döndürüyoruz
    //        if (appPage == null)
    //        {
    //            return ServiceResult<AppPageDto>.Fail("Home page not found.", HttpStatusCode.NotFound);
    //        }

    //        // Geçerli dildeki çeviriyi alıyoruz
    //        //var translation = appPage.AppPageTranslations
    //        //    .FirstOrDefault(t => t.LanguageCode == languageCode);

    //        //if (translation == null)
    //        //{
    //        //    return ServiceResult<AppPageDto>.Fail("Translation not found for the specified language.", HttpStatusCode.NotFound);
    //        //}

    //        // AppPage entity'sini DTO'ya dönüştürüyoruz
    //        var appPageDto = AppPageDto.FromEntity(appPage, _languageProvider.CurrentLanguage);

    //        return ServiceResult<AppPageDto>.Success(appPageDto, HttpStatusCode.OK);
    //    }
    //}
}
