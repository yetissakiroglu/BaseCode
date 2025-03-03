using AutoMapper;
using Economy.Application.Commands.AppMenus;
using Economy.Application.Dtos.AppMenuDtos;
using Economy.Application.Interfaces;
using Economy.Application.Queries.AppMenus;
using Economy.Application.Repositories.AppMenuRepositories;
using Economy.Core.Tools;
using Economy.Core.UnitOfWorks;
using Economy.Domain.Entites.EntityMenuItems;
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
            var appMenu = await _appMenuRepository.GetForReadAsync(x => x.Id == command.MenuId);
            // Eğer appMenu bulunamazsa, hata döndürüyoruz
            if (appMenu == null)
            {
                return ResponseModel<bool>.Fail("Menu bulunamadı", HttpStatusCode.NotFound);
            }
            await _appMenuRepository.DeleteAsync(appMenu);
            await _unitOfWork.CommitAsync();
            return ResponseModel<bool>.Success(true,HttpStatusCode.OK);
        }
              
        public async Task<ResponseModel<AppMenuDto>> GetForReadAsync(GetAppMenuByMenuIdQuery query)
        {
            var appMenu = await _appMenuRepository.GetForReadAsync(x => x.Id == query.MenuId, x => x.SubMenus, x => x.ParentMenu);

            // Eğer appMenu bulunamazsa, hata döndürüyoruz
            if (appMenu == null)
            {
                return ResponseModel<AppMenuDto>.Fail("Menu bulunamadı", HttpStatusCode.NotFound);
            }

            var appMenuDto = _mapper.Map<AppMenuDto>(appMenu);
            return ResponseModel<AppMenuDto>.Success(appMenuDto, HttpStatusCode.OK);
        }

        public async Task<ResponseModel<int>> InsertAsync(CreateAppMenuCommand command)
        {
            var insert = new AppMenu()
            {
                Title = command.Title,
                Slug = command.Slug,
                IsExternal = command.IsExternal,
                ParentMenuId = command.ParentMenuId,
            };
            await _appMenuRepository.AddAsync(insert);
            await _unitOfWork.CommitAsync();
            return ResponseModel<int>.Success(insert.Id,HttpStatusCode.OK);
        }

        public async Task<ResponseModel<AppMenuDto>> UpdateAsync(UpdateAppMenuCommand command)
        {
            var appMenu = await _appMenuRepository.GetForEditAsync(x => x.Id == command.Id);

            appMenu.Title = command.Title;
            appMenu.Slug = command.Slug;
            appMenu.IsExternal = command.IsExternal;
            appMenu.ParentMenuId = command.ParentMenuId;

            await _appMenuRepository.UpdateAsync(appMenu);
            await _unitOfWork.CommitAsync();

            var dto = _mapper.Map<AppMenuDto>(appMenu);
            return ResponseModel<AppMenuDto>.Success(dto,HttpStatusCode.OK);
        }

        public async Task<ResponseModel<List<AppMenuDto>>> WhereForReadAsync(GetAllAppMenuQuery query)
        {
            var appMenu = await _appMenuRepository.WhereForReadAsync(null, x => x.SubMenus, x => x.ParentMenu);
            var appMenuDto = _mapper.Map<List<AppMenuDto>>(appMenu);
            return ResponseModel<List<AppMenuDto>>.Success(appMenuDto, HttpStatusCode.OK);
        }

        public async Task<ResponseModel<List<AppMenuDto>>> WhereForReadAsync(GetAllAppMenuByParentMenuIdQuery query)
        {
            var appMenu = await _appMenuRepository.WhereForReadAsync(w => w.ParentMenuId == query.ParentMenuId, x => x.SubMenus, x => x.ParentMenu);
            var appMenuDto = _mapper.Map<List<AppMenuDto>>(appMenu);
            return ResponseModel<List<AppMenuDto>>.Success(appMenuDto, HttpStatusCode.OK);
        }
    }
}
