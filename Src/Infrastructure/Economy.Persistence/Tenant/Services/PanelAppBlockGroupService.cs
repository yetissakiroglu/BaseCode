using AutoMapper;
using Economy.Application.TenantUI.Dtos;
using Economy.Application.TenantUI.Dtos.AppBlockDtos;
using Economy.Application.TenantUI.Dtos.AppBlockGroupDtos;
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
    public class PanelAppBlockGroupService : IPanelAppBlockGroupService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityRepository<AppLanguage, int> _appLanguage;
        private readonly IEntityRepository<AppBlockGroup, int> _appBlockGroup;
        private readonly IEntityRepository<AppBlockGroupTranslation, int> _appBlockGroupTranslation;
        private readonly IEntityRepository<AppBlockGroupBlock, int> _appBlockGroupBlock;

        public PanelAppBlockGroupService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _appBlockGroup = unitOfWork.HotelEntityRepository<AppBlockGroup>();
            _appLanguage = unitOfWork.HotelEntityRepository<AppLanguage>();
            _appBlockGroupTranslation = unitOfWork.HotelEntityRepository<AppBlockGroupTranslation>();
            _appBlockGroupBlock = unitOfWork.HotelEntityRepository<AppBlockGroupBlock>();
            _mapper = mapper;
        }

        public async Task<ServiceResult<NoContent>> CreateBlockGroupAsync(AppBlockGroupDto vm, CancellationToken ct)
        {
            var ci = new AppBlockGroup
            {
                IsDeleted = false,
                ShowDescription = vm.ShowDescription,
                Columns = vm.Columns,
                IsActive = vm.IsActive,
                ShowTitle = vm.ShowTitle,
            };

            await _appBlockGroup.DataSet.AddAsync(ci, ct);
            await _unitOfWork.SaveHotelChangesAsync();

            foreach (var t in vm.Translations)
            {

                var tr = new AppBlockGroupTranslation
                {
                    AppBlockGroupId = ci.Id,
                    AppLanguageId = t.LanguageId,
                    IsDeleted = false,
                    Title = t.Title,
                    Description = t.Description,
                };
                await _appBlockGroupTranslation.DataSet.AddAsync(tr, ct);
            }

            await _unitOfWork.SaveHotelChangesAsync();
            return ServiceResult<NoContent>.Success(new NoContent() { Id = ci.Id });
        }

        public async Task<ServiceResult<NoContent>> DeleteBlockGroupAsync(int blockGroupId, CancellationToken ct)
        {
            var deletedBlocks = _appBlockGroup.DataSet.Where(x => x.Id == blockGroupId);
            _appBlockGroup.DataSet.RemoveRange(deletedBlocks);
            await _unitOfWork.SaveHotelChangesAsync();
            return ServiceResult<NoContent>.Success(new NoContent { Id = blockGroupId });
        }

        public async Task<ServiceResult<NoContent>> EnsureLanguageTabsAsync(AppBlockGroupDto vm, CancellationToken ct)
        {
            var exist = vm.Translations.Select(t => t.LanguageId).ToHashSet();
            var langs = await _appLanguage.DataSet.Where(x => !x.IsDeleted && x.IsActive)
                .Select(x => new { x.Id, x.Code, x.Icon, x.Name }).ToListAsync(ct);

            foreach (var l in langs)
                if (!exist.Contains(l.Id))
                    vm.Translations.Add(new AppBlockGroupTranslationDto { LanguageId = l.Id, LanguageCode = l.Code, LanguageIcon = l.Icon, LanguageName = l.Name });

            vm.Translations = [.. vm.Translations
                    .OrderByDescending(t => t.LanguageCode == "tr")
                    .ThenBy(t => t.LanguageId)];

            return ServiceResult<NoContent>.Success(null);
        }
        public async Task<ServiceResult<NoContent>> FillLanguagesAsync(AppBlockGroupDto vm, CancellationToken ct)
        {
            var langs = await _appLanguage.DataSet
              .Where(x => !x.IsDeleted && x.IsActive)
              .OrderByDescending(x => x.IsDefault)
              .ThenBy(x => x.Id)
              .Select(x => new { x.Id, x.Code, x.Icon,x.Name })
              .ToListAsync(ct);

            vm.Translations = langs.Select(l => new AppBlockGroupTranslationDto
            {
                LanguageId = l.Id,
                LanguageCode = l.Code,
                LanguageIcon = l.Icon,
                LanguageName = l.Name
            }).ToList();

            return ServiceResult<NoContent>.Success(null);
        }
        public async Task<ServiceResult<List<AppBlockGroupListDto>>> GetAllBlockGroupsListAsync(CancellationToken ct)
        {
            var defLangId = await _appLanguage.DataSet.Where(l => !l.IsDeleted && l.IsActive && l.IsDefault)
               .Select(l => l.Id).FirstOrDefaultAsync(ct);

            if (defLangId == 0)
                defLangId = await _appLanguage.DataSet.Where(l => !l.IsDeleted && l.IsActive)
                    .Select(l => l.Id).FirstOrDefaultAsync(ct);

            var list = await (from ci in _appBlockGroup.DataSet
                              where !ci.IsDeleted
                              join tr in _appBlockGroupTranslation.DataSet on ci.Id equals tr.AppBlockGroupId into trx
                              from tr in trx.Where(t => !t.IsDeleted && t.AppLanguageId == defLangId).DefaultIfEmpty()
                              orderby ci.ShowTitle, ci.Id
                              select new AppBlockGroupListDto
                              {
                                  Id = ci.Id,
                                  ShowDescription = ci.ShowDescription,
                                  Columns = ci.Columns,
                                  ShowTitle = ci.ShowTitle,
                                  Description = tr != null ? tr.Description : null,
                                  Title = tr != null ? tr.Title : null,
                                  IsActive = ci.IsActive
                              })
                              .ToListAsync(ct);


            foreach (var item in list)
            {
                var count = _appBlockGroupBlock.DataSet.Where(x => x.IsActive && !x.IsDeleted && x.AppBlockGroupId == item.Id).Count();
                item.BlockCount = count;
            }


            return ServiceResult<List<AppBlockGroupListDto>>.Success(list);
        }
        public async Task<ServiceResult<AppBlockGroupDto>> GetBlockGroupAsync(int blockGroupId, CancellationToken ct)
        {
            var b = await _appBlockGroup.DataSet.Include(x => x.Translations).FirstOrDefaultAsync(x => x.Id == blockGroupId && !x.IsDeleted, ct);
            if (b == null)
            {
                return ServiceResult<AppBlockGroupDto>.Empty();
            }

            var langs = await _appLanguage.DataSet.Where(x => x.IsActive && !x.IsDeleted).ToListAsync(cancellationToken: ct);
            var vm = new AppBlockGroupDto
            {
                Id = b.Id,
                ShowDescription = b.ShowDescription,
                Columns = b.Columns,
                ShowTitle = b.ShowTitle,
                IsActive = b.IsActive,
                Translations = [.. langs.Select(l =>
                {
                    var bt = b.Translations.FirstOrDefault(t => t.AppLanguageId == l.Id);
                    return new AppBlockGroupTranslationDto { Id = bt?.Id, Description = bt?.Description,Title=bt?.Title, LanguageId = l.Id, LanguageCode = l.Code, LanguageIcon = l.Icon, LanguageName = l.Name };
                })]
            };

            return ServiceResult<AppBlockGroupDto>.Success(vm);
        }
        public async Task<ServiceResult<NoContent>> UpdateBlockGroupAsync(int blockGroupId, AppBlockGroupDto vm, CancellationToken ct)
        {
            var ci = await _appBlockGroup.DataSet.FirstOrDefaultAsync(x => x.Id == blockGroupId && !x.IsDeleted, ct);
            if (ci is null)
            {
                return ServiceResult<NoContent>.Empty();
            }

            ci.IsActive = vm.IsActive;
            ci.ShowDescription = vm.ShowDescription;
            ci.ShowTitle = vm.ShowTitle;
            ci.Columns = vm.Columns;

            var existing = await _appBlockGroupTranslation.DataSet
                .Where(t => !t.IsDeleted && t.AppBlockGroupId == blockGroupId)
                .ToListAsync(ct);

            foreach (var t in vm.Translations)
            {
                var ex = existing.FirstOrDefault(x => x.AppLanguageId == t.LanguageId);
                if (ex is null)
                {
                    var tr = new AppBlockGroupTranslation
                    {
                        AppBlockGroupId = blockGroupId,
                        AppLanguageId = t.LanguageId,
                        Description = t.Description,
                        IsDeleted = false,
                        Title = t.Title
                    };
                    await _appBlockGroupTranslation.DataSet.AddAsync(tr, ct);
                }
                else
                {
                    ex.Title = t.Title;
                    ex.Description = t.Description;
                }
            }

            await _unitOfWork.SaveHotelChangesAsync();
            return ServiceResult<NoContent>.Success(new NoContent() { Id = ci.Id });
        }

























        //public async Task<ServiceResult<List<BlockItemDto>>> GetAllBlocksAsync(int? languageId, CancellationToken ct)
        //{
        //    var q = _block.DataSet.AsNoTracking()
        //        .Where(b => !b.IsDeleted && b.IsActive)
        //        .Select(b => new
        //        {
        //            b.Id,
        //            Title = b.Tag
        //        });

        //    var list = await q
        //        // null başlıkları sona at, sonra Id ile sabitle
        //        .OrderBy(x => x.Title == null)     // true > false, yani null’lar sona
        //        .ThenBy(x => x.Title)              // null olmayanları alfabetik
        //        .ThenBy(x => x.Id)
        //        .Select(x => new BlockItemDto(
        //            x.Id,
        //            x.Title ?? $"Block #{x.Id}"
        //        ))
        //        .ToListAsync(ct);


        //    return ServiceResult<List<BlockItemDto>>.Success(list);
        //}


        //public async Task<ServiceResult<List<GroupLayoutItemDto>>> GetGroupLayoutAsync(int groupId, CancellationToken ct)
        //{
        //    var q = await _blockGroupBlock.DataSet.AsNoTracking()
        //        .Where(x => x.AppBlockGroupId == groupId && !x.IsDeleted && x.IsActive)
        //        .OrderBy(x => x.SortOrder)
        //        .Select(x => new GroupLayoutItemDto(x.AppBlockId, x.SortOrder, x.Column))
        //        .ToListAsync(ct);

        //    return ServiceResult<List<GroupLayoutItemDto>>.Success(q);
        //}

        //public async Task<ServiceResult<NoContent>> SaveGroupLayoutAsync(SaveGroupLayoutRequest model, CancellationToken ct)
        //{
        //    // 1) Doğrulama
        //    if (model.GroupId <= 0) return ServiceResult<NoContent>.Failure("Geçersiz grup.");
        //    if (model.Items.Any(i => i.Column < 1 || i.Column > 12))
        //        return ServiceResult<NoContent>.Failure("Kolon değeri 1–12 aralığında olmalı.");

        //    var dupIds = model.Items.GroupBy(i => i.BlockId).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
        //    if (dupIds.Any()) return ServiceResult<NoContent>.Failure("Aynı blok bir grupta bir kez olabilir.");

        //    // 2) Mevcutlar
        //    var existing = await _blockGroupBlock.DataSet
        //        .Where(x => x.AppBlockGroupId == model.GroupId)
        //        .ToListAsync(ct);
        //    var map = existing.ToDictionary(x => x.AppBlockId, x => x);

        //    // 3) Ekle/Güncelle
        //    for (int i = 0; i < model.Items.Count; i++)
        //    {
        //        var it = model.Items[i];
        //        if (map.TryGetValue(it.BlockId, out var row))
        //        {
        //            row.SortOrder = it.SortOrder;
        //            row.Column = it.Column;
        //            row.IsActive = true;
        //            row.IsDeleted = false;
        //        }
        //        else
        //        {
        //            _blockGroupBlock.DataSet.Add(new AppBlockGroupBlock
        //            {
        //                AppBlockGroupId = model.GroupId,
        //                AppBlockId = it.BlockId,
        //                SortOrder = it.SortOrder,
        //                Column = it.Column,
        //                IsActive = true
        //            });
        //        }
        //    }

        //    // 4) Listede olmayanları soft-delete
        //    var keep = model.Items.Select(i => i.BlockId).ToHashSet();
        //    foreach (var row in existing)
        //    {
        //        if (!keep.Contains(row.AppBlockId))
        //        {
        //            row.IsActive = false;
        //            row.IsDeleted = true;
        //        }
        //    }

        //    await _unitOfWork.SaveHotelChangesAsync();
        //    return ServiceResult<NoContent>.Success(new NoContent { Id = 0 });
        //}


        //public async Task<List<BlockGroupDto>> GetGroupsAsync(bool includeItems = true)
        //{
        //    var q = _blockGroupRepository.DataSet.AsQueryable();

        //    var list = await q.OrderBy(x => x.ShowTitle).ToListAsync();
        //    return _mapper.Map<List<BlockGroupDto>>(list);
        //}
        //public async Task<BlockGroupDto?> GetGroupAsync(int id, bool includeItems = true)
        //{
        //    var q = _blockGroupRepository.DataSet.AsQueryable();

        //    var ent = await q.FirstOrDefaultAsync(x => x.Id == id);
        //    return ent == null ? null : _mapper.Map<BlockGroupDto>(ent);
        //}


        //public async Task<ServiceResult<NoContent>> CreateGroupAsync(BlockGroupDto vm, CancellationToken ct)
        //{

        //    var ci = new AppBlockGroup
        //    {
        //        IsDeleted = false,
        //        ShowDescription = false,
        //        Columns = BlockColumns.One,
        //        IsActive = false,
        //        ShowTitle = false,
        //    };

        //    await _blockGroupRepository.DataSet.AddAsync(ci, ct);
        //    await _unitOfWork.SaveHotelChangesAsync();

        //    foreach (var t in vm.Translations)
        //    {

        //        var tr = new AppBlockGroupTranslation
        //        {
        //            AppBlockGroupId = ci.Id,
        //            AppLanguageId = t.LanguageId,
        //            IsDeleted = false,
        //            Title = t.Title,
        //            Description = t.Description,
        //        };
        //        await _trRepo.DataSet.AddAsync(tr, ct);
        //    }

        //    await _unitOfWork.SaveHotelChangesAsync();
        //    return ServiceResult<NoContent>.Success(new NoContent() { Id = ci.Id });
        //}

        //public async Task UpdateGroupAsync(int id, BlockGroupDto dto)
        //{
        //    var ent = await _blockGroupRepository.DataSet.FirstOrDefaultAsync(x => x.Id == id);
        //    if (ent == null) return;

        //    ent.Columns = dto.Columns;
        //    ent.ShowTitle = dto.ShowTitle;
        //    ent.ShowDescription = dto.ShowDescription;
        //    await _unitOfWork.SaveHotelChangesAsync();
        //}

        //public async Task DeleteGroupAsync(int id)
        //{
        //    var ent = await _blockGroupRepository.DataSet
        //    .FirstOrDefaultAsync(x => x.Id == id);
        //    if (ent == null) return;
        //    _blockGroupRepository.DataSet.Remove(ent);
        //    await _unitOfWork.SaveHotelChangesAsync();
        //}


        //public async Task<ServiceResult<List<BlockGroupListDto>>> GetGroupsListAsync(CancellationToken ct)
        //{
        //    var defLangId = await _entityLanguageRepository.DataSet.Where(l => !l.IsDeleted && l.IsActive && l.IsDefault)
        //       .Select(l => l.Id).FirstOrDefaultAsync(ct);

        //    if (defLangId == 0)
        //        defLangId = await _entityLanguageRepository.DataSet.Where(l => !l.IsDeleted && l.IsActive)
        //            .Select(l => l.Id).FirstOrDefaultAsync(ct);

        //    var list = await (from ci in _blockGroupRepository.DataSet
        //                      where !ci.IsDeleted
        //                      join tr in _trRepo.DataSet on ci.Id equals tr.AppBlockGroupId into trx
        //                      from tr in trx.Where(t => !t.IsDeleted && t.AppLanguageId == defLangId).DefaultIfEmpty()
        //                      orderby ci.ShowTitle, ci.Id
        //                      select new BlockGroupListDto
        //                      {
        //                          Id = ci.Id,
        //                          ShowDescription = ci.ShowDescription,
        //                          Columns = ci.Columns,
        //                          ShowTitle = ci.ShowTitle,
        //                          Description = tr != null ? tr.Description : null,
        //                          Title = tr != null ? tr.Title : null,
        //                      })
        //                      .ToListAsync(ct);

        //    return ServiceResult<List<BlockGroupListDto>>.Success(list);
        //}

        //public async Task<ServiceResult<NoContent>> FillLanguagesAsync(BlockGroupDto vm, CancellationToken ct)
        //{
        //    var langs = await _entityLanguageRepository.DataSet
        //      .Where(x => !x.IsDeleted && x.IsActive)
        //      .OrderByDescending(x => x.IsDefault)
        //      .ThenBy(x => x.Id)
        //      .Select(x => new { x.Id, x.Code, x.Icon })
        //      .ToListAsync(ct);

        //    vm.Translations = langs.Select(l => new BlockGroupTranslationDto
        //    {
        //        LanguageId = l.Id,
        //        LanguageCode = l.Code,
        //        LanguageIcon = l.Icon
        //    }).ToList();

        //    return ServiceResult<NoContent>.Success(null);
        //}

        //public async Task<ServiceResult<NoContent>> EnsureLanguageTabsAsync(BlockGroupDto vm, CancellationToken ct)
        //{
        //    var exist = vm.Translations.Select(t => t.LanguageId).ToHashSet();
        //    var langs = await _entityLanguageRepository.DataSet.Where(x => !x.IsDeleted && x.IsActive)
        //        .Select(x => new { x.Id, x.Code, x.Icon }).ToListAsync(ct);

        //    foreach (var l in langs)
        //        if (!exist.Contains(l.Id))
        //            vm.Translations.Add(new BlockGroupTranslationDto { LanguageId = l.Id, LanguageCode = l.Code, LanguageIcon = l.Icon });

        //    vm.Translations = [.. vm.Translations
        //        .OrderByDescending(t => t.LanguageCode == "tr")
        //        .ThenBy(t => t.LanguageId)];

        //    return ServiceResult<NoContent>.Success(null);
        //}

        //public async Task<ServiceResult<BlockGroupDto>> GetGroupAsync(int id, CancellationToken ct)
        //{

        //    var langs = await _entityLanguageRepository.DataSet.Where(x => x.IsActive && !x.IsDeleted).OrderBy(x => x.Id).ToListAsync();

        //    //var ci = await _blockGroupRepository.DataSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
        //    var ci = await _blockGroupRepository.DataSet
        //      .Include(x => x.Translations)
        //      //.Include(x => x.AppBlocks).ThenInclude(b => b.Translations)
        //      .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);


        //    if (ci is null)
        //    {
        //        return ServiceResult<BlockGroupDto>.Empty();
        //    }

        //    var vm = new BlockGroupDto
        //    {
        //        Id = ci.Id,
        //        ShowDescription = ci.ShowDescription,
        //        Columns = ci.Columns,
        //        ShowTitle = ci.ShowTitle
        //    };

        //    await FillLanguagesAsync(vm, ct);

        //    var trs = await _trRepo.DataSet
        //        .Where(t => !t.IsDeleted && t.AppBlockGroupId == ci.Id)
        //        .ToListAsync(ct);

        //    foreach (var t in vm.Translations)
        //    {
        //        var hit = trs.FirstOrDefault(x => x.AppLanguageId == t.LanguageId);
        //        if (hit is null) continue;
        //        t.Id = hit.Id;
        //        t.Title = hit.Title;
        //        t.Description = hit.Description;
        //    }


        //    //vm.Blocks = ci.AppBlocks.Select(b => new BlockGroupBlockVm
        //    //{
        //    //    Id = b.Id,
        //    //    Type = b.Type,
        //    //    //SortOrder = b.SortOrder,
        //    //    IsActive = b.IsActive,
        //    //    //Stage = b.Stage,
        //    //    SharedJson = b.SharedJson,
        //    //    Tag = b.Tag,
        //    //    Translations = langs.Select(l =>
        //    //    {
        //    //        var bt = b.Translations.FirstOrDefault(t => t.AppLanguageId == l.Id);
        //    //        return new BlockGroupBlockTranslationVm { Id = bt?.Id, LanguageId = l.Id, LanguageCode = l.Code, LanguageIcon = l.Icon, LocalizedJson = bt?.LocalizedJson ?? "{}" };
        //    //    }).ToList()
        //    //}).ToList();



        //    return ServiceResult<BlockGroupDto>.Success(vm);
        //}

        //public async Task<ServiceResult<NoContent>> UpdateGroupAsync(int id, BlockGroupDto vm, CancellationToken ct)
        //{
        //    var ci = await _blockGroupRepository.DataSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
        //    if (ci is null)
        //    {
        //        return ServiceResult<NoContent>.Empty();
        //    }


        //    ci.ShowDescription = vm.ShowDescription;
        //    ci.Columns = vm.Columns;
        //    ci.ShowTitle = vm.ShowTitle;


        //    var existing = await _trRepo.DataSet
        //        .Where(t => !t.IsDeleted && t.AppBlockGroupId == id)
        //        .ToListAsync(ct);

        //    foreach (var t in vm.Translations)
        //    {
        //        var ex = existing.FirstOrDefault(x => x.AppLanguageId == t.LanguageId);
        //        if (ex is null)
        //        {
        //            var tr = new AppBlockGroupTranslation
        //            {
        //                AppBlockGroupId = id,
        //                AppLanguageId = t.LanguageId,
        //                IsDeleted = false,
        //                Title = t.Title,
        //                Description = t.Description
        //            };
        //            await _trRepo.DataSet.AddAsync(tr, ct);
        //        }
        //        else
        //        {
        //            ex.Description = t.Description;
        //            ex.Title = t.Title;

        //        }
        //    }

        //    await _unitOfWork.SaveHotelChangesAsync();
        //    return ServiceResult<NoContent>.Success(new NoContent() { Id = ci.Id });
        //}

        //public async Task<ServiceResult<NoContent>> DeleteGroupAsync(int id, CancellationToken ct)
        //{
        //    var ent = await _blockGroupRepository.DataSet.Include(x => x.Translations).FirstOrDefaultAsync(x => x.Id == id);
        //    if (ent == null)
        //    {
        //        return ServiceResult<NoContent>.Empty();
        //    }

        //    _blockGroupRepository.DataSet.Remove(ent);
        //    await _unitOfWork.SaveHotelChangesAsync();

        //    return ServiceResult<NoContent>.Success(new NoContent() { Id = id });
        //}

        //public async Task<ServiceResult<NoContent>> CreateUpdateGroupAndItemsAsync(BlockGroupDto vm, CancellationToken ct)
        //{
        //    var langs = await _entityLanguageRepository.DataSet.Where(x => x.IsActive).ToListAsync();
        //    if (langs is null)
        //    {
        //        return ServiceResult<NoContent>.Empty();
        //    }

        //    AppBlockGroup p;
        //    if (vm.Id == null)
        //    {
        //        p = new AppBlockGroup
        //        {
        //            IsActive = vm.IsActive,
        //            ShowDescription = vm.ShowDescription,
        //            Columns = vm.Columns,
        //            ShowTitle = vm.ShowTitle,
        //        };
        //        foreach (var t in vm.Translations)
        //            p.Translations.Add(new AppBlockGroupTranslation { AppLanguageId = t.LanguageId, Title = t.Title, Description = t.Description });
        //        _blockGroupRepository.DataSet.Add(p);
        //    }
        //    else
        //    {
        //        //p = await _blockGroupRepository.DataSet.Include(x => x.Translations).Include(x => x.AppBlocks).ThenInclude(x => x.Translations)
        //        //    .FirstAsync(x => x.Id == vm.Id.Value);
        //        //p.IsActive = vm.IsActive; p.Columns = vm.Columns; p.ShowDescription = vm.ShowDescription;

        //        //// Sayfa çevirileri
        //        //foreach (var l in langs)
        //        //{
        //        //    var incoming = vm.Translations.First(t => t.LanguageId == l.Id);
        //        //    var cur = p.Translations.FirstOrDefault(t => t.AppLanguageId == l.Id);
        //        //    if (cur == null) p.Translations.Add(new AppBlockGroupTranslation { AppLanguageId = l.Id, Title = incoming.Title, Description = incoming.Description });
        //        //    else { cur.Title = incoming.Title; cur.Description = incoming.Description; }
        //        //}

        //        //// Silinen bloklar
        //        //var keep = vm.Blocks.Where(b => b.Id.HasValue).Select(b => b.Id!.Value).ToHashSet();
        //        ////var toRemove = p.AppBlocks.Where(x => !keep.Contains(x.Id)).ToList();
        //        //_block.DataSet.RemoveRange(toRemove);
        //    }

        //    // Blok upsert + sıralama
        //    int order = 0;
        //    //foreach (var bvm in vm.Blocks)
        //    //{
        //    //    AppBlock e;
        //    //    if (bvm.Id == null)
        //    //    {
        //    //        e = new AppBlock
        //    //        {
        //    //            Type = bvm.Type,
        //    //            //SortOrder = order++,
        //    //            IsActive = bvm.IsActive,
        //    //            //Stage = bvm.Stage,
        //    //            SharedJson = bvm.SharedJson,
        //    //            Tag = bvm.Tag,
        //    //        };
        //    //        foreach (var bt in bvm.Translations)
        //    //            e.Translations.Add(new AppBlockTranslation { AppLanguageId = bt.LanguageId, LocalizedJson = bt.LocalizedJson });
        //    //        p.AppBlocks.Add(e);
        //    //    }
        //    //    else
        //    //    {
        //    //        e = p.AppBlocks.First(x => x.Id == bvm.Id.Value);
        //    //        e.Type = bvm.Type; 
        //    //        //e.SortOrder = order++;
        //    //        e.IsActive = bvm.IsActive; 
        //    //        //e.Stage = bvm.Stage;
        //    //        e.SharedJson = bvm.SharedJson; e.Tag = bvm.Tag;


        //    //        foreach (var l in langs)
        //    //        {
        //    //            var incoming = bvm.Translations.First(t => t.LanguageId == l.Id);
        //    //            var cur = e.Translations.FirstOrDefault(t => t.AppLanguageId == l.Id);
        //    //            if (cur == null) e.Translations.Add(new AppBlockTranslation { AppLanguageId = l.Id, LocalizedJson = incoming.LocalizedJson });
        //    //            else cur.LocalizedJson = incoming.LocalizedJson;
        //    //        }
        //    //    }
        //    //}
        //    //await _unitOfWork.SaveHotelChangesAsync();
        //    //return ServiceResult<NoContent>.Success(new NoContent() { Id = p.Id });
        //    return ServiceResult<NoContent>.Success(new NoContent() { Id = 0 });

        //}
    }
}