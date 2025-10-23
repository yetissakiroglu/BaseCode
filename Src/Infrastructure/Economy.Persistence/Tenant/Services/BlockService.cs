using AutoMapper;
using Economy.Application.TenantUI.Dtos;
using Economy.Application.TenantUI.Interfaces;
using Economy.Core.Enums;
using Economy.Core.Interfaces;
using Economy.Core.Tools;
using Economy.Core.Tools.Result;
using Economy.Domain.Entites.TenantEntity.EntityAppBlocks;
using Economy.Domain.Entites.TenantEntity.EntityAppLanguages;
using Microsoft.EntityFrameworkCore;

namespace Economy.Persistence.Tenant.Services
{
    public class BlockService : IBlockGroupService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityRepository<BlockGroup, int> _blockGroupRepository;
        private readonly IEntityRepository<BlockGroupTranslation, int> _trRepo;
        private readonly IEntityRepository<BlockGroupBlock, int> _blockGroupBlock;
        private readonly IEntityRepository<AppLanguage, int> _entityLanguageRepository;

        public BlockService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _blockGroupRepository = unitOfWork.HotelEntityRepository<BlockGroup>();
            _entityLanguageRepository = unitOfWork.HotelEntityRepository<AppLanguage>();
            _trRepo = unitOfWork.HotelEntityRepository<BlockGroupTranslation>();
            _blockGroupBlock = unitOfWork.HotelEntityRepository<BlockGroupBlock>();
            _mapper = mapper;
        }
        public async Task<List<BlockGroupDto>> GetGroupsAsync(bool includeItems = true)
        {
            var q = _blockGroupRepository.DataSet.AsQueryable();
         
            var list = await q.OrderBy(x => x.ShowTitle).ToListAsync();
            return _mapper.Map<List<BlockGroupDto>>(list);
        }


        public async Task<BlockGroupDto?> GetGroupAsync(int id, bool includeItems = true)
        {
            var q = _blockGroupRepository.DataSet.AsQueryable();
 
            var ent = await q.FirstOrDefaultAsync(x => x.Id == id);
            return ent == null ? null : _mapper.Map<BlockGroupDto>(ent);
        }


        public async Task<ServiceResult<NoContent>> CreateGroupAsync(BlockGroupDto vm, CancellationToken ct)
        {

            var ci = new BlockGroup
            {
                IsDeleted = false,
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
            ent.ShowTitle = dto.ShowTitle;
            ent.ShowDescription = dto.ShowDescription;
            await _unitOfWork.SaveHotelChangesAsync();
        }

        public async Task DeleteGroupAsync(int id)
        {
            var ent = await _blockGroupRepository.DataSet
            .FirstOrDefaultAsync(x => x.Id == id);
            if (ent == null) return;
            _blockGroupRepository.DataSet.Remove(ent);
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

            var langs = await _entityLanguageRepository.DataSet.Where(x => x.IsActive && !x.IsDeleted).OrderBy(x => x.Id).ToListAsync();

            //var ci = await _blockGroupRepository.DataSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
            var ci = await _blockGroupRepository.DataSet
              .Include(x => x.Translations)
              .Include(x => x.BlockGroupBlocks).ThenInclude(b => b.Translations)
              .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);


            if (ci is null)
            {
                return ServiceResult<BlockGroupDto>.Empty();
            }

            var vm = new BlockGroupDto
            {
                Id = ci.Id,
                ShowDescription = ci.ShowDescription,
                Columns = ci.Columns,
                ShowTitle = ci.ShowTitle
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


            vm.Blocks = ci.BlockGroupBlocks.OrderBy(b => b.SortOrder).Select(b => new BlockGroupBlockVm
            {
                Id = b.Id,
                Type = b.Type,
                SortOrder = b.SortOrder,
                IsActive = b.IsActive,
                Stage = b.Stage,
                SharedJson = b.SharedJson,
                Tag =b.Tag,
                Translations = langs.Select(l =>
                {
                    var bt = b.Translations.FirstOrDefault(t => t.AppLanguageId == l.Id);
                    return new BlockGroupBlockTranslationVm { Id = bt?.Id, LanguageId = l.Id,LanguageCode =l.Code,LanguageIcon =l.Icon , LocalizedJson = bt?.LocalizedJson ?? "{}" };
                }).ToList()
            }).ToList();



            return ServiceResult<BlockGroupDto>.Success(vm);
        }

        public async Task<ServiceResult<NoContent>> UpdateGroupAsync(int id, BlockGroupDto vm, CancellationToken ct)
        {
            var ci = await _blockGroupRepository.DataSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
            if (ci is null)
            {
                return ServiceResult<NoContent>.Empty();
            }


            ci.ShowDescription = vm.ShowDescription;
            ci.Columns = vm.Columns;
            ci.ShowTitle = vm.ShowTitle;


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
            var ent = await _blockGroupRepository.DataSet.Include(x => x.Translations).FirstOrDefaultAsync(x => x.Id == id);
            if (ent == null)
            {
                return ServiceResult<NoContent>.Empty();
            }

            _blockGroupRepository.DataSet.Remove(ent);
            await _unitOfWork.SaveHotelChangesAsync();

            return ServiceResult<NoContent>.Success(new NoContent() { Id = id });
        }

        public async Task<ServiceResult<NoContent>> CreateUpdateGroupAndItemsAsync(BlockGroupDto vm, CancellationToken ct)
        {
            var langs = await _entityLanguageRepository.DataSet.Where(x => x.IsActive).ToListAsync();
            if (langs is null)
            {
                return ServiceResult<NoContent>.Empty();
            }

            BlockGroup p;
            if (vm.Id == null)
            {
                p = new BlockGroup
                {
                    IsActive = vm.IsActive,
                    ShowDescription = vm.ShowDescription,
                    Columns = vm.Columns,
                    ShowTitle = vm.ShowTitle,
                };
                foreach (var t in vm.Translations)
                    p.Translations.Add(new BlockGroupTranslation { AppLanguageId = t.LanguageId, Title = t.Title, Description = t.Description });
                _blockGroupRepository.DataSet.Add(p);
            }
            else
            {
                p = await _blockGroupRepository.DataSet.Include(x => x.Translations).Include(x => x.BlockGroupBlocks).ThenInclude(x => x.Translations)
                    .FirstAsync(x => x.Id == vm.Id.Value);
                p.IsActive = vm.IsActive; p.Columns = vm.Columns; p.ShowDescription = vm.ShowDescription;

                // Sayfa çevirileri
                foreach (var l in langs)
                {
                    var incoming = vm.Translations.First(t => t.LanguageId == l.Id);
                    var cur = p.Translations.FirstOrDefault(t => t.AppLanguageId == l.Id);
                    if (cur == null) p.Translations.Add(new BlockGroupTranslation { AppLanguageId = l.Id, Title = incoming.Title, Description = incoming.Description });
                    else { cur.Title = incoming.Title; cur.Description = incoming.Description; }
                }

                // Silinen bloklar
                var keep = vm.Blocks.Where(b => b.Id.HasValue).Select(b => b.Id!.Value).ToHashSet();
                var toRemove = p.BlockGroupBlocks.Where(x => !keep.Contains(x.Id)).ToList();
                _blockGroupBlock.DataSet.RemoveRange(toRemove);
            }

            // Blok upsert + sıralama
            int order = 0;
            foreach (var bvm in vm.Blocks.OrderBy(x => x.SortOrder))
            {
                BlockGroupBlock e;
                if (bvm.Id == null)
                {
                    e = new BlockGroupBlock
                    {
                        Type = bvm.Type,
                        SortOrder = order++,
                        IsActive = bvm.IsActive,
                        Stage = bvm.Stage,
                        SharedJson = bvm.SharedJson,
                        Tag = bvm.Tag,
                    };
                    foreach (var bt in bvm.Translations)
                        e.Translations.Add(new BlockGroupBlockTranslation { AppLanguageId = bt.LanguageId, LocalizedJson = bt.LocalizedJson });
                    p.BlockGroupBlocks.Add(e);
                }
                else
                {
                    e = p.BlockGroupBlocks.First(x => x.Id == bvm.Id.Value);
                    e.Type = bvm.Type; e.SortOrder = order++; e.IsActive = bvm.IsActive; e.Stage = bvm.Stage; e.SharedJson = bvm.SharedJson; e.Tag = bvm.Tag;


                    foreach (var l in langs)
                    {
                        var incoming = bvm.Translations.First(t => t.LanguageId == l.Id);
                        var cur = e.Translations.FirstOrDefault(t => t.AppLanguageId == l.Id);
                        if (cur == null) e.Translations.Add(new BlockGroupBlockTranslation { AppLanguageId = l.Id, LocalizedJson = incoming.LocalizedJson });
                        else cur.LocalizedJson = incoming.LocalizedJson;
                    }
                }
            }
            await _unitOfWork.SaveHotelChangesAsync();
            return ServiceResult<NoContent>.Success(new NoContent() { Id = p.Id });

        }
    }
}