using AutoMapper;
using Economy.Application.Commands.AppMenus;
using Economy.Application.Dtos.AppMenuDtos;
using Economy.Application.Interfaces;
using Economy.Application.Queries.AppMenus;
using Economy.Application.Repositories.AppMenuRepositories;
using Economy.Core.Interfaces;
using Economy.Core.Tools;
using Economy.Domain.Entites.EntityAppMenus;
using Economy.Domain.Entites.EntityMenuItems;
using LoggingLibrary.Attributes;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Economy.Persistence.Services
{
    public class AppMenuService(IAppMenuRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
       : IAppMenuService
    {
        private readonly IAppMenuRepository _appMenuRepository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        public ResponseModel<bool> Delete(DeleteAppMenuCommand command)
        {
            var appMenu = _appMenuRepository.GetForRead(x => x.Id == command.MenuId, x => x.Translations);
            // Eğer appMenu bulunamazsa, hata döndürüyoruz
            if (appMenu == null)
            {
                return ResponseModel<bool>.Fail("Menu bulunamadı", HttpStatusCode.NotFound);
            }
            _appMenuRepository.Delete(appMenu);
            _unitOfWork.SaveChangesAsync();
            return ResponseModel<bool>.Success(true, HttpStatusCode.OK);
        }
        //[Cache(Duration = 30)]  // BU DOĞRU!
        public ResponseModel<AppMenuDto> GetForRead(GetAppMenuByMenuIdQuery query)
        {
            var appMenu = _appMenuRepository.GetForRead(x => x.Id == query.MenuId, x => x.SubMenus, x => x.ParentMenu, x => x.Translations);

            // Eğer data bulunamazsa, hata döndürüyoruz
            if (appMenu == null)
            {
                return ResponseModel<AppMenuDto>.Fail("kayıt bulunamadı", HttpStatusCode.NotFound);
            }

            var appMenuDto = _mapper.Map<AppMenuDto>(appMenu);
            return ResponseModel<AppMenuDto>.Success(appMenuDto, HttpStatusCode.OK);
        }
        public ResponseModel<int> Insert(CreateAppMenuCommand command)
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
            _appMenuRepository.Add(insert);
             _unitOfWork.SaveChangesAsync();
            return ResponseModel<int>.Success(insert.Id, HttpStatusCode.OK);
        }
        public  ResponseModel<AppMenuDto> Update(UpdateAppMenuCommand command)
        {
            var appMenu = _appMenuRepository.GetForEdit(x => x.Id == command.Id, x => x.SubMenus, x => x.ParentMenu, x => x.Translations);

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

            _appMenuRepository.Update(appMenu);
             _unitOfWork.SaveChangesAsync();

            var dto = _mapper.Map<AppMenuDto>(appMenu);
            return ResponseModel<AppMenuDto>.Success(dto, HttpStatusCode.OK);
        }
        //[Cache(Duration = 30)]  // BU DOĞRU!
        public ResponseModel<List<AppMenuDto>> WhereForRead(GetAllAppMenuQuery query)
        {
            var appMenu = _appMenuRepository.WhereForRead(null, x => x.SubMenus, x => x.ParentMenu, x => x.Translations);
            var appMenuDto = _mapper.Map<List<AppMenuDto>>(appMenu);
            return ResponseModel<List<AppMenuDto>>.Success(appMenuDto, HttpStatusCode.OK);
        }
        [Log("Menü WhereForReadAsync alındı.")]
        //[Cache(Duration = 30)]  // BU DOĞRU!
        public ResponseModel<List<AppMenuDto>> WhereForRead(GetAllAppMenuByParentMenuIdQuery query)
        {
            var appMenu = _appMenuRepository.WhereForReadFunc(w => w.ParentMenuId == query.ParentMenuId,
            q => q.Include(x => x.Translations.Where(w => w.AppLanguage.Code == query.LanguageCode)).Include(w=>w.Translations).ThenInclude(w=>w.AppLanguage));
            var appMenuDto = _mapper.Map<List<AppMenuDto>>(appMenu);
            return ResponseModel<List<AppMenuDto>>.Success(appMenuDto, HttpStatusCode.OK);
        }
    }
}
