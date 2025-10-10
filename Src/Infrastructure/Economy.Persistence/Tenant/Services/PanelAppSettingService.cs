using AutoMapper;
using Economy.Application.TenantUI.Dtos.AppSettingDtos;
using Economy.Application.TenantUI.Interfaces;
using Economy.Core.Enums;
using Economy.Core.Interfaces;
using Economy.Core.Tools;
using Economy.Core.Tools.Result;
using Economy.Domain.Entites.TenantEntity.EntityAppLanguages;
using Economy.Domain.Entites.TenantEntity.EntityAppPages;
using Economy.Domain.Entites.TenantEntity.EntityAppSettings;
using Economy.Panel.Application.Extensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Economy.Persistence.Tenant.Services
{
    public class PanelAppSettingService : IPanelAppSettingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityRepository<AppSetting, int> _appSettingRepository;
        private readonly IEntityRepository<AppSettingTranslation, int> _appSettingTranslationRepository;

        private readonly IMapper _mapper;
        private readonly IValidator<AppSettingCreateEditDto> _validator;
        private readonly IEntityRepository<AppLanguage, int> _entityLanguageRepository;

        public PanelAppSettingService(IUnitOfWork unitOfWork, IMapper mapper, IValidator<AppSettingCreateEditDto> validator)
        {
            _unitOfWork = unitOfWork;
            _appSettingRepository = unitOfWork.HotelEntityRepository<AppSetting>();
            _appSettingTranslationRepository = unitOfWork.HotelEntityRepository<AppSettingTranslation>();
            _entityLanguageRepository = unitOfWork.HotelEntityRepository<AppLanguage>();
            _mapper = mapper;
            _validator = validator;
        }
        public async Task<ServiceResult<NoContent>> Create(AppSettingCreateEditDto vm, CancellationToken ct)
        {
            var validation = _validator.Validate(vm);
            if (!validation.IsValid)
            {
                return ServiceResult<NoContent>.Failure(
                    "Doğrulama hatası",
                    validationErrors: validation.ToValidationDictionary()
                );
            }

            var ci = new AppSetting
            {
                IsDeleted = false,
            };
            await _appSettingRepository.DataSet.AddAsync(ci, ct);
            await _unitOfWork.SaveHotelChangesAsync();

            foreach (var t in vm.Translations)
            {
                var tr = new AppSettingTranslation
                {
                    AppSettingId = ci.Id,
                    AppLanguageId = t.AppLanguageId,
                    IsDeleted = false,
                    Title = t.Title,
                    Description = t.Description,
                    MetaSlogan = t.MetaSlogan,
                    MetaTitle = t.MetaTitle,
                    MetaDescription = t.MetaDescription,
                };
                await _appSettingTranslationRepository.DataSet.AddAsync(tr, ct);
            }
            await _unitOfWork.SaveHotelChangesAsync();
            return ServiceResult<NoContent>.Success(null);
        }
        public async Task<ServiceResult<NoContent>> Edit(int id, AppSettingCreateEditDto vm, CancellationToken ct)
        {
            var validation = _validator.Validate(vm);
            if (!validation.IsValid)
            {
                return ServiceResult<NoContent>.Failure(
                    "Doğrulama hatası",
                    validationErrors: validation.ToValidationDictionary()
                );
            }

            var ci = await _appSettingRepository.DataSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
            if (ci is null)
            {
                return ServiceResult<NoContent>.Empty();
            }

            var existing = await _appSettingTranslationRepository.DataSet
                .Where(t => !t.IsDeleted && t.AppSettingId == id)
                .ToListAsync(ct);

            foreach (var t in vm.Translations)
            {
                var ex = existing.FirstOrDefault(x => x.AppLanguageId == t.AppLanguageId);
                if (ex is null)
                {

                    var tr = new AppSettingTranslation
                    {
                        AppSettingId = id,
                        AppLanguageId = t.AppLanguageId,
                        IsDeleted = false,
                        Title = t.Title,
                        MetaTitle = t.MetaTitle,
                        MetaDescription = t.MetaDescription,
                        MetaSlogan = t.MetaSlogan,
                        Description = t.Description,
                    };
                    await _appSettingTranslationRepository.DataSet.AddAsync(tr, ct);
                }
                else
                {
                    ex.Title = t.Title;
                    ex.MetaTitle = t.MetaTitle;
                    ex.MetaDescription = t.MetaDescription;
                    ex.MetaSlogan = t.MetaSlogan;
                    ex.Description = t.Description;
                }
            }

            await _unitOfWork.SaveHotelChangesAsync();
            return ServiceResult<NoContent>.Success(null);
        }
        public async Task<ServiceResult<NoContent>> EnsureLanguageTabsAsync(AppSettingCreateEditDto vm, CancellationToken ct)
        {
            var exist = vm.Translations.Select(t => t.AppLanguageId).ToHashSet();
            var langs = await _entityLanguageRepository.DataSet.Where(x => !x.IsDeleted && x.IsActive)
                .Select(x => new { x.Id, x.Code, x.Icon }).ToListAsync(ct);

            foreach (var l in langs)
                if (!exist.Contains(l.Id))
                    vm.Translations.Add(new AppSettingTranslationDto { AppLanguageId = l.Id, AppLanguageCode = l.Code, AppLanguageIcon = l.Icon });

            vm.Translations = vm.Translations
                .OrderByDescending(t => t.AppLanguageCode == "tr")
                .ThenBy(t => t.AppLanguageId)
                .ToList();

            return ServiceResult<NoContent>.Success(null);
        }
        public async Task<ServiceResult<NoContent>> FillLanguagesAsync(AppSettingCreateEditDto vm, CancellationToken ct)
        {
            var langs = await _entityLanguageRepository.DataSet
              .Where(x => !x.IsDeleted && x.IsActive)
              .OrderByDescending(x => x.IsDefault)
              .ThenBy(x => x.Id)
              .Select(x => new { x.Id, x.Code, x.Icon })
              .ToListAsync(ct);

            vm.Translations = langs.Select(l => new AppSettingTranslationDto
            {
                AppLanguageId = l.Id,
                AppLanguageCode = l.Code,
                AppLanguageIcon = l.Icon

            }).ToList();

            return ServiceResult<NoContent>.Success(null);
        }
        public async Task<ServiceResult<AppSettingDto>> GetAppSettingAsync(bool isDeleted, CancellationToken ct)
        {
            var entity = await _appSettingRepository.DataSet.Include(x => x.Translations).Where(x => x.IsDeleted == isDeleted)
                                .OrderByDescending(x => x.Id)
                                .FirstOrDefaultAsync(ct);

            if (entity == null)
                return ServiceResult<AppSettingDto>.Empty();

            var dto = _mapper.Map<AppSettingDto>(entity);
            return ServiceResult<AppSettingDto>.Success(dto);
        }

    }

}
