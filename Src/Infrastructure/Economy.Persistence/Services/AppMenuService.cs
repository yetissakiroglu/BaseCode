using AutoMapper;
using Economy.Application.Commands.AppMenus;
using Economy.Application.Dtos;
using Economy.Application.Exceptions;
using Economy.Application.Interfaces;
using Economy.Application.Queries.AppMenus;
using Economy.Application.Repositories.AppMenuRepositories;
using Economy.Application.UnitOfWorks;
using Economy.Domain.Entites.EntityMenuItems;
using System.Linq.Expressions;
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
            var appMenu = await _appMenuRepository.GetForReadAsync(x => x.Id == query.Id);
            if (appMenu == null)
            {
                throw new AppMenuNotFoundException(query.Id);
            }
            var appMenuDto = _mapper.Map<AppMenuDto>(appMenu);
            return ServiceResult<AppMenuDto>.Success(appMenuDto, HttpStatusCode.OK);
        }

        public Task<ServiceResult<AppMenuDto>> InsertAsync(CreateAppMenuCommand command)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<AppMenuDto>> UpdateAsync(UpdateAppMenuCommand command)
        {
            throw new NotImplementedException();
        }

        public Task<List<AppMenuDto>> WhereForReadAsync(GetAllAppMenusQuery query)
        {
            throw new NotImplementedException();
        }


        //public async Task<AppMenuDto> CreateAppMenuAsync(CreateAppMenuCommand command)
        //{
        //    var appMenu = new AppMenu
        //    {
        //        Title = command.Title,
        //        Slug = command.Slug,
        //        IsExternal = command.IsExternal,
        //        ParentMenuId = command.ParentMenuId
        //    };

        //    await _appMenuRepository.AddAsync(appMenu);
        //    await _unitOfWork.CommitAsync();

        //    return _mapper.Map<AppMenuDto>(appMenu);
        //}

        //public async Task<AppMenuDto> GetAppMenuByIdAsync(int id)
        //{
        //    var appMenu = await _appMenuRepository.GetByIdAsync(id);
        //    if (appMenu == null)
        //    {
        //        throw new AppMenuNotFoundException(id);
        //    }

        //    return _mapper.Map<AppMenuDto>(appMenu);
        //}

        //public async Task<IEnumerable<AppMenuDto>> GetAllAppMenusAsync()
        //{
        //    //var appMenus = await _appMenuRepository.GetAllAsync();
        //    var appMenus = new List<AppMenu>();
        //    return _mapper.Map<IEnumerable<AppMenuDto>>(appMenus);
        //}

        //public async Task<AppMenuDto> UpdateAppMenuAsync(UpdateAppMenuCommand command)
        //{
        //    var appMenu = await _appMenuRepository.GetByIdAsync(command.Id);
        //    if (appMenu == null)
        //    {
        //        throw new AppMenuNotFoundException(command.Id);
        //    }

        //    appMenu.Title = command.Title;
        //    appMenu.Slug = command.Slug;
        //    appMenu.IsExternal = command.IsExternal;
        //    appMenu.ParentMenuId = command.ParentMenuId;

        //    _appMenuRepository.UpdateAsync(appMenu);
        //    await _unitOfWork.CommitAsync();

        //    return _mapper.Map<AppMenuDto>(appMenu);
        //}

        //public async Task DeleteAppMenuAsync(int id)
        //{
        //    var appMenu = await _appMenuRepository.GetByIdAsync(id);
        //    if (appMenu == null)
        //    {
        //        throw new AppMenuNotFoundException(id);
        //    }

        //    //_appMenuRepository.Delete(appMenu);
        //    //await _appMenuRepository.SaveChangesAsync();
        //}
    }




}
