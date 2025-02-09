namespace Economy.Persistence.Services
{
    //public class AppLanguageServices : Service<AppLanguage, int>, IAppLanguageServices
    //{
    //	private readonly IUnitOfWork _unitOfWork;
    //	private readonly IEntityRepository<AppLanguage, int> _readRepository;
    //	private readonly ILanguageProvider _languageProvider;

    //	public AppLanguageServices(IUnitOfWork unitOfWork, IEntityRepository<AppLanguage, int> readRepository, ILanguageProvider languageProvider)
    //		: base(readRepository, unitOfWork)
    //	{
    //		_unitOfWork = unitOfWork;
    //		_readRepository = readRepository;
    //		_languageProvider = languageProvider;
    //	}

    //	public async Task<ServiceResult<AppLanguageDto>> AppLanguageAsync(string languageCode)
    //       {
    //           var modelEntitie = await _readRepository.Table
    //               .Where(mi => mi.LanguageCode == languageCode && mi.IsEnabled).FirstOrDefaultAsync();

    //           // Eğer slideEntities boşsa hata döndür
    //           if (modelEntitie is null)
    //           {
    //               return ServiceResult<AppLanguageDto>.Fail("Ürün bulunamadı", HttpStatusCode.NotFound);
    //           }

    //           var modelDto = AppLanguageDto.FromEntity(modelEntitie);

    //           return ServiceResult<AppLanguageDto>.Success(modelDto, HttpStatusCode.OK);
    //       }

    //       public async Task<ServiceResult<AppLanguageDto>> AppLanguageDefaultAsync()
    //       {

    //           var modelEntitie = await _readRepository.Table
    //               .Where(mi => mi.IsDefault && mi.IsEnabled).FirstOrDefaultAsync();

    //           // Eğer slideEntities boşsa hata döndür
    //           if (modelEntitie is null)
    //           {
    //               return ServiceResult<AppLanguageDto>.Fail("Ürün bulunamadı", HttpStatusCode.NotFound);
    //           }

    //           var modelDto = AppLanguageDto.FromEntity(modelEntitie);

    //           return ServiceResult<AppLanguageDto>.Success(modelDto, HttpStatusCode.OK);
    //       }

    //       public async Task<ServiceResult<List<AppLanguageDto>>> AppLanguageListAsync()
    //       { 

    //           var modelEntities = await _readRepository.Table.
    //               Where(mi=>mi.IsEnabled)
    //               .ToListAsync();

    //           // Eğer slideEntities boşsa hata döndür
    //           if (!modelEntities.Any())
    //           {
    //               return ServiceResult<List<AppLanguageDto>>.Fail("Ürün bulunamadı", HttpStatusCode.NotFound);
    //           }

    //           var modelDtos = AppLanguageDto.List(modelEntities)
    //            .ToList();

    //           return ServiceResult<List<AppLanguageDto>>.Success(modelDtos, HttpStatusCode.OK);
    //       }
    //   }

}
