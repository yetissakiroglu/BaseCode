using AutoMapper;
using Economy.Application.TenantUI.Dtos.AppBlockDtos;
using Economy.Application.TenantUI.Interfaces;
using Economy.Core.Enums;
using Economy.Core.Extensions;
using Economy.Core.Interfaces;
using Economy.Core.Tools;
using Economy.Core.Tools.Result;
using Economy.Domain.Entites.TenantEntity.EntityAppBlocks;
using Economy.Domain.Entites.TenantEntity.EntityAppLanguages;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Economy.Persistence.Tenant.Services
{
    public class PanelAppBlockService : IPanelAppBlockService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityRepository<AppLanguage, int> _appLanguage;
        private readonly IEntityRepository<AppBlock, int> _appBlock;
        private readonly IEntityRepository<AppBlockTranslation, int> _appBlockTranslation;
        private readonly IEntityRepository<AppBlockGroupBlock, int> _appBlockGroupBlock;
        private readonly IMapper _mapper;
        public PanelAppBlockService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _appLanguage = unitOfWork.HotelEntityRepository<AppLanguage>();
            _appBlock = unitOfWork.HotelEntityRepository<AppBlock>();
            _appBlockTranslation = unitOfWork.HotelEntityRepository<AppBlockTranslation>();
            _appBlockGroupBlock = unitOfWork.HotelEntityRepository<AppBlockGroupBlock>();
        }

        public async Task<ServiceResult<NoContent>> CreateBlockAsync(AppBlockDto vm, CancellationToken ct)
        {
            var ci = new AppBlock
            {
                IsDeleted = false,
                SharedJson = vm.SharedJson,
                Tag = vm.Tag,
                Type = vm.Type,
                IsActive = vm.IsActive,
            };

            await _appBlock.DataSet.AddAsync(ci, ct);
            await _unitOfWork.SaveHotelChangesAsync();

            foreach (var t in vm.Translations)
            {
                var tr = new AppBlockTranslation
                {
                    AppBlockId = ci.Id,
                    AppLanguageId = t.LanguageId,
                    IsDeleted = false,
                    LocalizedJson = t.LocalizedJson
                };
                await _appBlockTranslation.DataSet.AddAsync(tr, ct);
            }

            await _unitOfWork.SaveHotelChangesAsync();
            return ServiceResult<NoContent>.Success(new NoContent() { Id = ci.Id });
        }
        public async Task<ServiceResult<NoContent>> DeleteBlockAsync(int blockId, CancellationToken ct)
        {
            var deletedBlocks = _appBlock.DataSet.Where(x => x.Id == blockId);
            _appBlock.DataSet.RemoveRange(deletedBlocks);
            await _unitOfWork.SaveHotelChangesAsync();
            return ServiceResult<NoContent>.Success(new NoContent { Id = blockId });
        }
        public async Task<ServiceResult<NoContent>> EditBlockAsync(int blockId, AppBlockDto vm, CancellationToken ct)
        {
            var ci = await _appBlock.DataSet.FirstOrDefaultAsync(x => x.Id == blockId && !x.IsDeleted, ct);
            if (ci is null)
            {
                return ServiceResult<NoContent>.Empty();
            }

            ci.IsActive = vm.IsActive;
            ci.SharedJson = vm.SharedJson;
            ci.Tag = vm.Tag;
            ci.Type = vm.Type;

            var existing = await _appBlockTranslation.DataSet
                .Where(t => !t.IsDeleted && t.AppBlockId == blockId)
                .ToListAsync(ct);

            foreach (var t in vm.Translations)
            {
                var ex = existing.FirstOrDefault(x => x.AppLanguageId == t.LanguageId);
                if (ex is null)
                {
                    var tr = new AppBlockTranslation
                    {
                        AppBlockId = blockId,
                        AppLanguageId = t.LanguageId,
                        LocalizedJson = t.LocalizedJson
                    };
                    await _appBlockTranslation.DataSet.AddAsync(tr, ct);
                }
                else
                {
                    ex.LocalizedJson = t.LocalizedJson;
                }
            }

            await _unitOfWork.SaveHotelChangesAsync();
            return ServiceResult<NoContent>.Success(new NoContent() { Id = ci.Id });
        }
        public async Task<ServiceResult<NoContent>> EnsureLanguageTabsAsync(AppBlockDto vm, CancellationToken ct)
        {
            var exist = vm.Translations.Select(t => t.LanguageId).ToHashSet();
            var langs = await _appLanguage.DataSet.Where(x => !x.IsDeleted && x.IsActive)
                .Select(x => new { x.Id, x.Code, x.Icon, x.Name }).ToListAsync(ct);

            foreach (var l in langs)
                if (!exist.Contains(l.Id))
                    vm.Translations.Add(new AppBlockTranslationDto { LanguageId = l.Id, LanguageCode = l.Code, LanguageIcon = l.Icon, LanguageName = l.Name, LocalizedJson = vm.Type.GetLocalizedJson() });

            vm.Translations = vm.Translations
                .OrderByDescending(t => t.LanguageCode == "tr")
                .ThenBy(t => t.LanguageId)
                .ToList();

            return ServiceResult<NoContent>.Success(null);
        }
        public async Task<ServiceResult<NoContent>> FillLanguagesAsync(AppBlockDto vm, CancellationToken ct)
        {
            var langs = await _appLanguage.DataSet
               .Where(x => !x.IsDeleted && x.IsActive)
               .OrderByDescending(x => x.IsDefault)
               .ThenBy(x => x.Id)
               .Select(x => new { x.Id, x.Code, x.Icon, x.Name })
               .ToListAsync(ct);

            vm.Translations = langs.Select(l => new AppBlockTranslationDto
            {
                LanguageId = l.Id,
                LanguageCode = l.Code,
                LanguageIcon = l.Icon,
                LanguageName = l.Name
            }).ToList();

            return ServiceResult<NoContent>.Success(null);
        }
        public async Task<ServiceResult<NoContent>> FillLanguagesAsync(AppBlockDto vm, BlockType type, CancellationToken ct)
        {
            var langs = await _appLanguage.DataSet
               .Where(x => !x.IsDeleted && x.IsActive)
               .OrderByDescending(x => x.IsDefault)
               .ThenBy(x => x.Id)
               .Select(x => new { x.Id, x.Code, x.Icon, x.Name })
               .ToListAsync(ct);

            vm.Type = type;
            vm.SharedJson = type.GetSharedJson();

            vm.Translations = langs.Select(l => new AppBlockTranslationDto
            {
                LanguageId = l.Id,
                LanguageCode = l.Code,
                LanguageIcon = l.Icon,
                LanguageName = l.Name,
                LocalizedJson = type.GetLocalizedJson()
            }).ToList();

            return ServiceResult<NoContent>.Success(null);
        }
        public async Task<ServiceResult<AppBlockListDto>> GetAllBlocksAsync(BlockType? type, int page, int size, string? q)
        {
            var query = _appBlock.DataSet.AsQueryable();
            if (type.HasValue) query = query.Where(x => x.Type == type.Value);
            if (!string.IsNullOrWhiteSpace(q)) query = query.Where(x => x.SharedJson.Contains(q));

            var total = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * size).Take(size)
                .Select(x => new AppBlockListItemDto
                {
                    Id = x.Id,
                    Type = x.Type,
                    IsActive = x.IsActive,
                    Tag = x.Tag

                }).ToListAsync();

            var r = new AppBlockListDto
            {
                Items = items,
                Page = page,
                Size = size,
                Total = total,
                FilterType = type,
                Q = q
            };
            return ServiceResult<AppBlockListDto>.Success(r);
        }
        public async Task<ServiceResult<List<AppBlockMiniDto>>> GetAllMiniBlocksAsync(CancellationToken ct)
        {
            var q = _appBlock.DataSet.AsNoTracking()
                .Where(b => !b.IsDeleted && b.IsActive)
                .Select(b => new
                {
                    b.Id,
                    Title = b.Tag
                });


            var list = await q
                .OrderBy(x => x.Title == null)   
                .ThenBy(x => x.Title)            
                .ThenBy(x => x.Id)
                .Select(x => new AppBlockMiniDto(
                    x.Id,
                    x.Title ?? $"Block #{x.Id}"
                ))
                .ToListAsync(ct);


            return ServiceResult<List<AppBlockMiniDto>>.Success(list);
        }
        public async Task<ServiceResult<AppBlockDto>> GetBlocksAsync(int blockId, CancellationToken ct)
        {
            var b = await _appBlock.DataSet.Include(x => x.Translations).FirstOrDefaultAsync(x => x.Id == blockId, ct);
            if (b == null)
            {
                return ServiceResult<AppBlockDto>.Empty();
            }

            var langs = await _appLanguage.DataSet.Where(x => x.IsActive && !x.IsDeleted).ToListAsync();

            var vm = new AppBlockDto
            {
                Id = b.Id,
                Type = b.Type,
                IsActive = b.IsActive,
                SharedJson = b.SharedJson,
                Tag = b.Tag,
                Translations = langs.Select(l =>
                {
                    var bt = b.Translations.FirstOrDefault(t => t.AppLanguageId == l.Id);
                    return new AppBlockTranslationDto { Id = bt?.Id, LanguageId = l.Id, LanguageCode = l.Code, LanguageIcon = l.Icon,LanguageName=l.Name, LocalizedJson = bt?.LocalizedJson ?? b.Type.GetLocalizedJson() };
                }).ToList()
            };

            return ServiceResult<AppBlockDto>.Success(vm);
        }
    }
}
