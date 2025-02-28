using AutoMapper;
using Economy.Application.Commands.AppMenus;
using Economy.Application.Dtos.AppMenuDtos;
using Economy.Application.Interfaces;
using Economy.Application.Queries.AppMenus;
using Economy.Application.Repositories.AppMenuRepositories;
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

        public Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<bool>> DeleteAsync(AppMenuDto model)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<AppMenuDto>> GetForReadAsync(GetAppMenuQuery query)
        {
            var appMenu = await _appMenuRepository.GetForReadAsync(x => x.Id == query.Id, x => x.SubMenus, x => x.ParentMenu);

            if (appMenu == null)
            {

            }

            var appMenuDto = _mapper.Map<AppMenuDto>(appMenu);
            return ServiceResult<AppMenuDto>.Success(appMenuDto, HttpStatusCode.OK);
        }

        public async Task<ServiceResult<int>> InsertAsync(CreateAppMenuCommand command)
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
            return ServiceResult<int>.Success(insert.Id,HttpStatusCode.OK);
        }

        public async Task<ServiceResult<AppMenuDto>> UpdateAsync(UpdateAppMenuCommand command)
        {
            var appMenu = await _appMenuRepository.GetForEditAsync(x => x.Id == command.Id);

            appMenu.Title = command.Title;
            appMenu.Slug = command.Slug;
            appMenu.IsExternal = command.IsExternal;
            appMenu.ParentMenuId = command.ParentMenuId;

            await _appMenuRepository.UpdateAsync(appMenu);
            await _unitOfWork.CommitAsync();

            var dto = _mapper.Map<AppMenuDto>(appMenu);
            return ServiceResult<AppMenuDto>.Success(dto,HttpStatusCode.OK);
        }

        public async Task<ServiceResult<List<AppMenuDto>>> WhereForReadAsync(GetAllAppMenuQuery query)
        {
            var appMenu = await _appMenuRepository.WhereForEditAsync(null, x => x.SubMenus, x => x.ParentMenu);
            var appMenuDto = _mapper.Map<List<AppMenuDto>>(appMenu);
            return ServiceResult<List<AppMenuDto>>.Success(appMenuDto, HttpStatusCode.OK);
        }
    }




}
