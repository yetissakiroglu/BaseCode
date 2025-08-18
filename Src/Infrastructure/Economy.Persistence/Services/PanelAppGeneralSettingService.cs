using Economy.Application.Dtos.AppGeneralSettingDtos;
using Economy.Application.Interfaces;
using Economy.Core.Interfaces;
using Economy.Core.Tools.Result;
using Economy.Domain.Entites.AppEntities;
using System.Net;

public class PanelAppGeneralSettingService : IPanelAppGeneralSettingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEntityRepository<AppGeneralSetting, int> _appGeneralSettingRepository;

    public PanelAppGeneralSettingService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _appGeneralSettingRepository = unitOfWork.DefaultEntityRepository<AppGeneralSetting>();
    }

    public async Task<ServiceResult<AppGeneralSettingDto>> CreateGeneralSettingAsync(AppGeneralSettingCreateDto modelDto)
    {
        if (modelDto == null)
        {
            return ServiceResult<AppGeneralSettingDto>.Failure(
                message: "Geçersiz veri gönderildi.",
                statusCode: (int)HttpStatusCode.BadRequest);
        }

        // Basit sanitizasyon
        var entity = new AppGeneralSetting
        {
            SiteName = (modelDto.SiteName ?? string.Empty).Trim(),
            Domain = modelDto.Domain?.Trim(),
            Theme = string.IsNullOrWhiteSpace(modelDto.Theme) ? "light" : modelDto.Theme.Trim(),
            LogoUrl = modelDto.LogoUrl?.Trim(),
            MetaTitleSuffix = modelDto.MetaTitleSuffix?.Trim(),
            DefaultMetaDescription = modelDto.DefaultMetaDescription?.Trim(),
        };

        _appGeneralSettingRepository.Add(entity);
        await _unitOfWork.SaveDefaultChangesAsync();

        var dto = MapToDto(entity);
        return ServiceResult<AppGeneralSettingDto>.Success(
            data: dto,
            message: "Genel ayarlar başarıyla oluşturuldu.",
            statusCode: (int)HttpStatusCode.Created);
    }

    public async Task<ServiceResult<AppGeneralSettingDto>> DeleteGeneralSettingAsync(int Id)
    {
        if (Id <= 0)
        {
            return ServiceResult<AppGeneralSettingDto>.Failure(
                message: "Geçersiz Id.",
                statusCode: (int)HttpStatusCode.BadRequest);
        }

        var entity = _appGeneralSettingRepository.GetForEdit(x => x.Id == Id && !x.IsDeleted);
        if (entity == null)
        {
            return ServiceResult<AppGeneralSettingDto>.Failure(
                message: "Kayıt bulunamadı.",
                statusCode: (int)HttpStatusCode.NotFound);
        }

        _appGeneralSettingRepository.Delete(entity);
        await _unitOfWork.SaveDefaultChangesAsync();

        var dto = MapToDto(entity);
        return ServiceResult<AppGeneralSettingDto>.Success(
            data: dto,
            message: "Kayıt başarıyla silindi.",
            statusCode: (int)HttpStatusCode.OK);
    }

    public Task<ServiceResult<AppGeneralSettingDto>> GetGeneralSettingAsync(int Id)
    {
        if (Id <= 0)
        {
            return Task.FromResult(ServiceResult<AppGeneralSettingDto>.Failure(
                message: "Geçersiz Id.",
                statusCode: (int)HttpStatusCode.BadRequest));
        }

        var entity = _appGeneralSettingRepository.GetForRead(x => x.Id == Id && !x.IsDeleted);
        if (entity == null)
        {
            return Task.FromResult(ServiceResult<AppGeneralSettingDto>.Failure(
                message: "Kayıt bulunamadı.",
                statusCode: (int)HttpStatusCode.NotFound));
        }

        var dto = MapToDto(entity);
        return Task.FromResult(ServiceResult<AppGeneralSettingDto>.Success(dto));
    }

    public Task<ServiceResult<List<AppGeneralSettingDto>>> GetGeneralSettingListAsync()
    {
        var list = _appGeneralSettingRepository
            .WhereForRead(x => !x.IsDeleted)
            .Select(x => new AppGeneralSettingDto
            {
                Id = x.Id,
                SiteName = x.SiteName,
                Domain = x.Domain,
                Theme = x.Theme,
                LogoUrl = x.LogoUrl,
                MetaTitleSuffix = x.MetaTitleSuffix,
                DefaultMetaDescription = x.DefaultMetaDescription,

            })
            .ToList();

        return Task.FromResult(ServiceResult<List<AppGeneralSettingDto>>.Success(list));
    }

    public async Task<ServiceResult<AppGeneralSettingDto>> UpdateGeneralSettingAsync(AppGeneralSettingEditDto modelDto)
    {
        if (modelDto == null || modelDto.Id <= 0)
        {
            return ServiceResult<AppGeneralSettingDto>.Failure(
                message: "Geçersiz veri gönderildi.",
                statusCode: (int)HttpStatusCode.BadRequest);
        }

        var entity = _appGeneralSettingRepository.GetForEdit(x => x.Id == modelDto.Id && !x.IsDeleted);
        if (entity == null)
        {
            return ServiceResult<AppGeneralSettingDto>.Failure(
                message: "Kayıt bulunamadı.",
                statusCode: (int)HttpStatusCode.NotFound);
        }

        // Güncelleme (trim + fallback)
        entity.SiteName = (modelDto.SiteName ?? entity.SiteName)?.Trim();
        entity.Domain = modelDto.Domain?.Trim();
        entity.Theme = string.IsNullOrWhiteSpace(modelDto.Theme) ? entity.Theme : modelDto.Theme.Trim();
        entity.LogoUrl = modelDto.LogoUrl?.Trim();
        entity.MetaTitleSuffix = modelDto.MetaTitleSuffix?.Trim();
        entity.DefaultMetaDescription = modelDto.DefaultMetaDescription?.Trim();

        _appGeneralSettingRepository.Update(entity);
        await _unitOfWork.SaveDefaultChangesAsync();

        var dto = MapToDto(entity);
        return ServiceResult<AppGeneralSettingDto>.Success(
            data: dto,
            message: "Genel ayarlar başarıyla güncellendi.",
            statusCode: (int)HttpStatusCode.OK);
    }

    // ------------------------
    // Private helpers
    // ------------------------
    private static AppGeneralSettingDto MapToDto(AppGeneralSetting e) => new()
    {
        Id = e.Id,
        SiteName = e.SiteName,
        Domain = e.Domain,
        Theme = e.Theme,
        LogoUrl = e.LogoUrl,
        MetaTitleSuffix = e.MetaTitleSuffix,
        DefaultMetaDescription = e.DefaultMetaDescription,
    
    };
}
