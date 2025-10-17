using AutoMapper;
using Economy.Application.TenantUI.Dtos;
using Economy.Application.TenantUI.Dtos.AppPageDtos;
using Economy.Application.TenantUI.Interfaces;
using Economy.Core.Dtos.Custom;
using Economy.Core.Enums;
using Economy.Core.Interfaces;
using Economy.Core.Tools;
using Economy.Core.Tools.Result;
using Economy.Domain.Entites.TenantEntity.EntityAppBlocks;
using Economy.Domain.Entites.TenantEntity.EntityAppLanguages;
using Economy.Domain.Entites.TenantEntity.EntityAppPages;
using Microsoft.EntityFrameworkCore;

namespace Economy.Persistence.Tenant.Services
{
    public class BlockService : IBlockGroupService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityRepository<BlockGroup, int> _blockGroupRepository;
        private readonly IEntityRepository<BlockGroupTranslation, int> _trRepo;



        private readonly IEntityRepository<BlockItem, int> _blockItemRepository;
        private readonly IEntityRepository<BlockItemImage, int> _blockItemImageRepository;
        private readonly IEntityRepository<AppLanguage, int> _entityLanguageRepository;



        public BlockService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _blockGroupRepository = unitOfWork.HotelEntityRepository<BlockGroup>();
            _blockItemRepository = unitOfWork.HotelEntityRepository<BlockItem>();
            _blockItemImageRepository = unitOfWork.HotelEntityRepository<BlockItemImage>();
            _entityLanguageRepository = unitOfWork.HotelEntityRepository<AppLanguage>();
            _trRepo = unitOfWork.HotelEntityRepository<BlockGroupTranslation>();
            _mapper = mapper;
        }
        public async Task<List<BlockGroupDto>> GetGroupsAsync(bool includeItems = true)
        {
            var q = _blockGroupRepository.DataSet.AsQueryable();
            if (includeItems)
                q = q.Include(x => x.Items).ThenInclude(i => i.Gallery);


            var list = await q.OrderBy(x => x.ShowTitle).ToListAsync();
            return _mapper.Map<List<BlockGroupDto>>(list);
        }


        public async Task<BlockGroupDto?> GetGroupAsync(int id, bool includeItems = true)
        {
            var q = _blockGroupRepository.DataSet.AsQueryable();
            if (includeItems)
                q = q.Include(x => x.Items).ThenInclude(i => i.Gallery);


            var ent = await q.FirstOrDefaultAsync(x => x.Id == id);
            return ent == null ? null : _mapper.Map<BlockGroupDto>(ent);
        }


        public async Task<ServiceResult<NoContent>> CreateGroupAsync(BlockGroupDto vm, CancellationToken ct)
        {

            var ci = new BlockGroup
            {
                IsDeleted = false,
                DefaultImageMode = ImageMode.CoverOnly,
                ShowDescription = false,
                Columns = BlockColumns.One,
                IsActive = false,
                ShowTitle = false,
            };

            await _blockGroupRepository.DataSet.AddAsync(ci, ct);
            await _unitOfWork.SaveHotelChangesAsync();

            foreach (var t in vm.Translations)
            {

                var tr = new BlockGroupTranslation
                {
                    BlockGroupId = ci.Id,
                    AppLanguageId = t.LanguageId,
                    IsDeleted = false,
                    Title = t.Title,
                    Description = t.Description,
                };
                await _trRepo.DataSet.AddAsync(tr, ct);
            }

            await _unitOfWork.SaveHotelChangesAsync();
            return ServiceResult<NoContent>.Success(new NoContent() { Id = ci.Id });
        }

        public async Task UpdateGroupAsync(int id, BlockGroupDto dto)
        {
            var ent = await _blockGroupRepository.DataSet.FirstOrDefaultAsync(x => x.Id == id);
            if (ent == null) return;

            ent.Columns = dto.Columns;
            ent.DefaultImageMode = dto.DefaultImageMode;
            ent.ShowTitle = dto.ShowTitle;
            ent.ShowDescription = dto.ShowDescription;
            ent.PageId = dto.PageId;

            await _unitOfWork.SaveHotelChangesAsync();
        }

        public async Task DeleteGroupAsync(int id)
        {
            var ent = await _blockGroupRepository.DataSet.Include(x => x.Items).ThenInclude(i => i.Gallery)
            .FirstOrDefaultAsync(x => x.Id == id);
            if (ent == null) return;
            _blockGroupRepository.DataSet.Remove(ent);
            await _unitOfWork.SaveHotelChangesAsync();
        }


        public async Task<int> AddItemAsync(int groupId, BlockItemDto dto)
        {
            var group = await _blockGroupRepository.DataSet.FirstOrDefaultAsync(x => x.Id == groupId);
            if (group == null) return 0;


            var item = _mapper.Map<BlockItem>(dto);
            item.BlockGroupId = groupId;
            _blockItemRepository.DataSet.Add(item);
            await _unitOfWork.SaveHotelChangesAsync();

            // Gallery url’lerini kaydet
            if (dto.Gallery?.Any() == true)
            {
                var imgs = dto.Gallery.Select((url, idx) => new BlockItemImage
                {
                    BlockItemId = item.Id,
                    ImageUrl = url,
                    SortOrder = idx
                });
                _blockItemImageRepository.DataSet.AddRange(imgs);
                await _unitOfWork.SaveHotelChangesAsync();
            }


            return item.Id;
        }
        public async Task UpdateItemAsync(int itemId, BlockItemDto dto)
        {
            var item = await _blockItemRepository.DataSet.Include(x => x.Gallery).FirstOrDefaultAsync(x => x.Id == itemId);
            if (item == null) return;


            item.Title = dto.Title;
            item.Summary = dto.Summary;
            item.CoverImage = dto.CoverImage;
            item.ImageModeOverride = dto.ImageModeOverride;
            item.LinkType = dto.LinkType;
            item.LinkedPageId = dto.LinkedPageId;
            item.ExternalUrl = dto.ExternalUrl;
            item.Target = dto.Target;
            item.ColumnsOverride = dto.ColumnsOverride;
            item.SortOrder = dto.SortOrder;
            item.IsActive = dto.IsActive;


            // Gallery’yi sıfırla ve yeniden yaz
            _blockItemImageRepository.DataSet.RemoveRange(item.Gallery);
            if (dto.Gallery?.Any() == true)
            {
                var imgs = dto.Gallery.Select((url, idx) => new BlockItemImage
                {
                    BlockItemId = item.Id,
                    ImageUrl = url,
                    SortOrder = idx
                });
                await _blockItemImageRepository.DataSet.AddRangeAsync(imgs);
            }


            await _unitOfWork.SaveHotelChangesAsync();
        }


        public async Task DeleteItemAsync(int itemId)
        {
            var item = await _blockItemRepository.DataSet.Include(x => x.Gallery).FirstOrDefaultAsync(x => x.Id == itemId);
            if (item == null) return;
            _blockItemRepository.DataSet.Remove(item);
            await _unitOfWork.SaveHotelChangesAsync();
        }


        public async Task SortItemsAsync(int groupId, List<(int itemId, int sortOrder)> sortPairs)
        {
            var items = await _blockItemRepository.DataSet.Where(x => x.BlockGroupId == groupId).ToListAsync();
            foreach (var (itemId, sort) in sortPairs)
            {
                var it = items.FirstOrDefault(x => x.Id == itemId);
                if (it != null) it.SortOrder = sort;
            }
            await _unitOfWork.SaveHotelChangesAsync();
        }

        public async Task<ServiceResult<List<BlockGroupListDto>>> GetGroupsListAsync(CancellationToken ct)
        {
            var defLangId = await _entityLanguageRepository.DataSet.Where(l => !l.IsDeleted && l.IsActive && l.IsDefault)
               .Select(l => l.Id).FirstOrDefaultAsync(ct);

            if (defLangId == 0)
                defLangId = await _entityLanguageRepository.DataSet.Where(l => !l.IsDeleted && l.IsActive)
                    .Select(l => l.Id).FirstOrDefaultAsync(ct);

            var list = await (from ci in _blockGroupRepository.DataSet
                              where !ci.IsDeleted
                              join tr in _trRepo.DataSet on ci.Id equals tr.BlockGroupId into trx
                              from tr in trx.Where(t => !t.IsDeleted && t.AppLanguageId == defLangId).DefaultIfEmpty()
                              orderby ci.ShowTitle, ci.Id
                              select new BlockGroupListDto
                              {
                                  Id = ci.Id,
                                  DefaultImageMode = ci.DefaultImageMode,
                                  ShowDescription = ci.ShowDescription,
                                  Columns = ci.Columns,
                                  ShowTitle = ci.ShowTitle,
                                  Description = tr != null ? tr.Description : null,
                                  Title = tr != null ? tr.Title : null,
                              })
                              .ToListAsync(ct);

            return ServiceResult<List<BlockGroupListDto>>.Success(list);
        }

        public async Task<ServiceResult<NoContent>> FillLanguagesAsync(BlockGroupDto vm, CancellationToken ct)
        {
            var langs = await _entityLanguageRepository.DataSet
              .Where(x => !x.IsDeleted && x.IsActive)
              .OrderByDescending(x => x.IsDefault)
              .ThenBy(x => x.Id)
              .Select(x => new { x.Id, x.Code, x.Icon })
              .ToListAsync(ct);

            vm.Translations = langs.Select(l => new BlockGroupTranslationDto
            {
                LanguageId = l.Id,
                LanguageCode = l.Code,
                LanguageIcon = l.Icon
            }).ToList();

            return ServiceResult<NoContent>.Success(null);
        }

        public async Task<ServiceResult<NoContent>> EnsureLanguageTabsAsync(BlockGroupDto vm, CancellationToken ct)
        {
            var exist = vm.Translations.Select(t => t.LanguageId).ToHashSet();
            var langs = await _entityLanguageRepository.DataSet.Where(x => !x.IsDeleted && x.IsActive)
                .Select(x => new { x.Id, x.Code, x.Icon }).ToListAsync(ct);

            foreach (var l in langs)
                if (!exist.Contains(l.Id))
                    vm.Translations.Add(new BlockGroupTranslationDto { LanguageId = l.Id, LanguageCode = l.Code, LanguageIcon = l.Icon });

            vm.Translations = [.. vm.Translations
                .OrderByDescending(t => t.LanguageCode == "tr")
                .ThenBy(t => t.LanguageId)];

            return ServiceResult<NoContent>.Success(null);
        }

        public async Task<ServiceResult<BlockGroupDto>> GetGroupAsync(int id, CancellationToken ct)
        {
            var ci = await _blockGroupRepository.DataSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
            if (ci is null)
            {
                return ServiceResult<BlockGroupDto>.Empty();
            }

            var vm = new BlockGroupDto
            {
                Id = ci.Id,
                DefaultImageMode = ci.DefaultImageMode,
                ShowDescription = ci.ShowDescription,
                Columns = ci.Columns,
                ShowTitle = ci.ShowTitle,
                PageId = ci.PageId,
            };




            await FillLanguagesAsync(vm, ct);

            var trs = await _trRepo.DataSet
                .Where(t => !t.IsDeleted && t.BlockGroupId == ci.Id)
                .ToListAsync(ct);

            foreach (var t in vm.Translations)
            {
                var hit = trs.FirstOrDefault(x => x.AppLanguageId == t.LanguageId);
                if (hit is null) continue;

                t.Id = hit.Id;
                t.Title = hit.Title;
                t.Description = hit.Description;
            }

            return ServiceResult<BlockGroupDto>.Success(vm);
        }

        public async Task<ServiceResult<NoContent>> UpdateGroupAsync(int id, BlockGroupDto vm, CancellationToken ct)
        {
            var ci = await _blockGroupRepository.DataSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
            if (ci is null)
            {
                return ServiceResult<NoContent>.Empty();
            }


            ci.DefaultImageMode = vm.DefaultImageMode;
            ci.ShowDescription = vm.ShowDescription;
            ci.Columns = vm.Columns;
            ci.ShowTitle = vm.ShowTitle;
            ci.PageId = vm.PageId;


            var existing = await _trRepo.DataSet
                .Where(t => !t.IsDeleted && t.BlockGroupId == id)
                .ToListAsync(ct);

            foreach (var t in vm.Translations)
            {
                var ex = existing.FirstOrDefault(x => x.AppLanguageId == t.LanguageId);
                if (ex is null)
                {
                    var tr = new BlockGroupTranslation
                    {
                        BlockGroupId = id,
                        AppLanguageId = t.LanguageId,
                        IsDeleted = false,
                        Title = t.Title,
                        Description = t.Description
                    };
                    await _trRepo.DataSet.AddAsync(tr, ct);
                }
                else
                {
                    ex.Description = t.Description;
                    ex.Title = t.Title;

                }
            }

            await _unitOfWork.SaveHotelChangesAsync();
            return ServiceResult<NoContent>.Success(new NoContent() { Id = ci.Id });
        }

        public async Task<ServiceResult<NoContent>> DeleteGroupAsync(int id, CancellationToken ct)
        {
            var ent = await _blockGroupRepository.DataSet.Include(x=>x.Translations).Include(x => x.Items).ThenInclude(i => i.Gallery).FirstOrDefaultAsync(x => x.Id == id);
            if (ent == null)
            {
                return ServiceResult<NoContent>.Empty();
            }
            
            _blockGroupRepository.DataSet.Remove(ent);
            await _unitOfWork.SaveHotelChangesAsync();

            return ServiceResult<NoContent>.Success(new NoContent() { Id = id });
        }
    }
}