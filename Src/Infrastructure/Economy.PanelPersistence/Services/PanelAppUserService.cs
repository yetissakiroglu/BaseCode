using Economy.Base.Application.Dtos.BaseModels;
using Economy.Core.Dtos;
using Economy.Core.Tools;
using Economy.Core.Tools.Models;
using Economy.Panel.Application.Interfaces;
using Economy.Panel.Application.Repositories;
using System.Net;

namespace Economy.Panel.Persistence.Services
{
    public class PanelAppUserService : IPanelAppUserService
    {
        private readonly PanelAppUserRepository _panelAppUserRepository;

        public PanelAppUserService(PanelAppUserRepository panelAppUserRepository)
        {
            _panelAppUserRepository = panelAppUserRepository;
        }

        public async Task<ResponseModel<AppUserDto>> CreateUser(AppUserCreateDto userCreateDto)
        {
            return await _panelAppUserRepository.CreateUser(userCreateDto);
        }

        public async Task<ResponseModel<Token>> LoginAsync(SignIn signIn)
        {
            return await _panelAppUserRepository.LoginAsync(signIn);
        }

        public ResponseModel<List<AppUserListDto>> UserList(bool IsDeleted)
        {
            var result = _panelAppUserRepository.WhereForRead(w=>w.IsDeleted ==IsDeleted);
            
            return new ResponseModel<List<AppUserListDto>>()
            {
                IsSuccess = true,
                Data = result.Select(s => new AppUserListDto
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email,
                    PhoneNumber = s.PhoneNumber,
                    IsDefaultAdmin = s.IsDefaultAdmin
                }).ToList(),
                Message = new ResultMessage("Kullanıcı Listesi Başarıyla Getirildi"),
                Notification = Core.Enums.NotificationType.Success,
                Status = HttpStatusCode.OK
            };

        }
    }
}
