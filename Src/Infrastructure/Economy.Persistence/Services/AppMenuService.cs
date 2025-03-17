﻿using AutoMapper;
using Economy.Application.Commands.AppMenus;
using Economy.Application.Dtos.AppMenuDtos;
using Economy.Application.Interfaces;
using Economy.Application.Queries.AppMenus;
using Economy.Application.Repositories.AppMenuRepositories;
using Economy.Core.Tools;
using Economy.Core.UnitOfWorks;
using Economy.Domain.Entites.EntityAppMenus;
using Economy.Domain.Entites.EntityMenuItems;
using LoggingLibrary.Attributes;
using System.Net;

namespace Economy.Persistence.Services
{
    public class AppMenuService(IAppMenuRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
       : IAppMenuService
    {
        private readonly IAppMenuRepository _appMenuRepository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task<ResponseModel<bool>> DeleteAsync(DeleteAppMenuCommand command)
        {
            var appMenu = await _appMenuRepository.GetForReadAsync(x => x.Id == command.MenuId, x => x.Translations);
            // Eğer appMenu bulunamazsa, hata döndürüyoruz
            if (appMenu == null)
            {
                return ResponseModel<bool>.Fail("Menu bulunamadı", HttpStatusCode.NotFound);
            }
            await _appMenuRepository.DeleteAsync(appMenu);
            await _unitOfWork.CommitAsync();
            return ResponseModel<bool>.Success(true, HttpStatusCode.OK);
        }
        public async Task<ResponseModel<AppMenuDto>> GetForReadAsync(GetAppMenuByMenuIdQuery query)
        {
            var appMenu = await _appMenuRepository.GetForReadAsync(x => x.Id == query.MenuId, x => x.SubMenus, x => x.ParentMenu, x => x.Translations);

            // Eğer data bulunamazsa, hata döndürüyoruz
            if (appMenu == null)
            {
                return ResponseModel<AppMenuDto>.Fail("kayıt bulunamadı", HttpStatusCode.NotFound);
            }

            var appMenuDto = _mapper.Map<AppMenuDto>(appMenu);
            return ResponseModel<AppMenuDto>.Success(appMenuDto, HttpStatusCode.OK);
        }
        public async Task<ResponseModel<int>> InsertAsync(CreateAppMenuCommand command)
        {
            var insert = new AppMenu()
            {
                IsExternal = command.IsExternal,
                ParentMenuId = command.ParentMenuId,
                Translations = new List<AppMenuTranslation>
        {
            new AppMenuTranslation
            {
                AppLanguageId = command.AppLanguageId, // Kullanıcıdan gelen dil kodu
                Title = command.Title,
                Url = command.Url
            }
        }
            };
            await _appMenuRepository.AddAsync(insert);
            await _unitOfWork.CommitAsync();
            return ResponseModel<int>.Success(insert.Id, HttpStatusCode.OK);
        }
        public async Task<ResponseModel<AppMenuDto>> UpdateAsync(UpdateAppMenuCommand command)
        {
            var appMenu = await _appMenuRepository.GetForEditAsync(x => x.Id == command.Id, x => x.SubMenus, x => x.ParentMenu, x => x.Translations);

            if (appMenu == null)
            {
                return ResponseModel<AppMenuDto>.Fail("Menu not found", HttpStatusCode.NotFound);
            }

            // Mevcut çeviriyi al veya yeni ekle
            var translation = appMenu.Translations.FirstOrDefault(t => t.AppLanguageId == command.AppLanguageId);
            if (translation != null)
            {
                translation.Title = command.Title;
                translation.Url = command.Url;
            }
            else
            {
                appMenu.Translations.Add(new AppMenuTranslation
                {
                    AppLanguageId = command.AppLanguageId,
                    Title = command.Title,
                    Url = command.Url
                });
            }

            appMenu.IsExternal = command.IsExternal;
            appMenu.ParentMenuId = command.ParentMenuId;

            await _appMenuRepository.UpdateAsync(appMenu);
            await _unitOfWork.CommitAsync();

            var dto = _mapper.Map<AppMenuDto>(appMenu);
            return ResponseModel<AppMenuDto>.Success(dto, HttpStatusCode.OK);
        }

        public async Task<ResponseModel<List<AppMenuDto>>> WhereForReadAsync(GetAllAppMenuQuery query)
        {
            var appMenu = await _appMenuRepository.WhereForReadAsync(null, x => x.SubMenus, x => x.ParentMenu, x => x.Translations);
            var appMenuDto = _mapper.Map<List<AppMenuDto>>(appMenu);
            return ResponseModel<List<AppMenuDto>>.Success(appMenuDto, HttpStatusCode.OK);
        }
        [Log("Menü WhereForReadAsync alındı.")]
        public async Task<ResponseModel<List<AppMenuDto>>> WhereForReadAsync(GetAllAppMenuByParentMenuIdQuery query)
        {
            var appMenu = await _appMenuRepository.WhereForReadAsync(w => w.ParentMenuId == query.ParentMenuId,x => x.SubMenus,x => x.ParentMenu,x => x.Translations.Where(w => w.AppLanguage.Code == query.LanguageCode));
            var appMenuDto = _mapper.Map<List<AppMenuDto>>(appMenu);
            return ResponseModel<List<AppMenuDto>>.Success(appMenuDto, HttpStatusCode.OK);
        }
    }
}
