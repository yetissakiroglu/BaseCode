using Economy.Base.Application.Dtos.BaseModels;
using Economy.Core.Dtos;
using Economy.Core.Interfaces;
using Economy.Core.Tools;
using Economy.Core.Tools.Models;
using Economy.Domain.Entites.Identities;
using Economy.Panel.Application.Interfaces;
using Economy.Panel.Application.Repositories;
using System.Net;

namespace Economy.Panel.Persistence.Services
{
    public class PanelAppUserService : IPanelAppUserService
    {
        private readonly PanelAppUserRepository _panelAppUserRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PanelAppUserService(PanelAppUserRepository panelAppUserRepository, IUnitOfWork unitOfWork)
        {
            _panelAppUserRepository = panelAppUserRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseModel<AppUserDto>> CreateUser(AppUserCreateDto userCreateDto)
        {
            return await _panelAppUserRepository.CreateUser(userCreateDto);
        }

        public ResponseModel<AppUserDto> DeleteUser(int Id)
        {
            var result = _panelAppUserRepository.GetForEdit(w => w.Id == Id && w.IsDeleted == false);
            if (result == null)
            {
                return new ResponseModel<AppUserDto>
                {
                    IsSuccess = false,
                    Message = new ResultMessage("Kullanıcı bulunamadı."),
                    Status = HttpStatusCode.NotFound
                };
            }
            result.IsDeleted = true;
            _panelAppUserRepository.Update(result);
            _unitOfWork.SaveChanges();
            return new ResponseModel<AppUserDto>
            {
                IsSuccess = true,
                Message = new ResultMessage("Kullanıcı başarıyla silindi."),
                Status = HttpStatusCode.OK
            };
        }

        public ResponseModel<AppUserDto> EditUser(AppUserEditDto userEditDto)
        {
            var user = _panelAppUserRepository.GetForEdit(x => x.Id == userEditDto.Id);

            if (user == null)
            {
                return new ResponseModel<AppUserDto>
                {
                    IsSuccess = false,
                    Message = new ResultMessage("Kullanıcı bulunamadı."),
                    Status = HttpStatusCode.NotFound
                };
            }

            var userUpdateModel = new AppUser
            {
                Id = user.Id,
                FirstName = userEditDto.FirstName,
                LastName = userEditDto.LastName,
                Email = userEditDto.Email,
                PhoneNumber = userEditDto.PhoneNumber,
                UserName = user.UserName, // Değişmemesi isteniyor
                TenantId = user.TenantId  // Değişmemesi isteniyor
            };

            _panelAppUserRepository.Update(userUpdateModel);
            _unitOfWork.SaveChanges();
            return new ResponseModel<AppUserDto>
            {
                IsSuccess = false,
                Message = new ResultMessage("Kullanıcı güncellenemedi."),
                Status = HttpStatusCode.BadRequest,
                Data = new AppUserDto
                {
                    Id = userUpdateModel.Id,
                    FirstName = userUpdateModel.FirstName,
                    LastName = userUpdateModel.LastName,
                    Email = userUpdateModel.Email,
                    PhoneNumber = userUpdateModel.PhoneNumber,
                    UserName = user.UserName, // Değişmemesi isteniyor
                    TenantId = user.TenantId  // Değişmemesi isteniyor
                },
            };
           
        }

        public ResponseModel<AppUserDto> GetUser(int id, bool isDeleted)
        {
            var result = _panelAppUserRepository.GetForRead(w => w.Id == id && w.IsDeleted == isDeleted);
            var resultModel = new ResponseModel<AppUserDto>();
            return new ResponseModel<AppUserDto>()
            {
                IsSuccess = true,
                Data = new AppUserDto
                {
                    Id = result.Id,
                    FirstName = result.FirstName,
                    LastName = result.LastName,
                    Email = result.Email,
                    PhoneNumber = result.PhoneNumber,
                    UserName = result.UserName,
                    TenantId = result.TenantId
                },
                Message = new ResultMessage("Kullanıcı Başarıyla Getirildi"),
                Notification = Core.Enums.NotificationType.Success,
                Status = HttpStatusCode.OK
            }; 
        }

        public async Task<ResponseModel<Token>> LoginAsync(SignIn signIn)
        {
            return await _panelAppUserRepository.LoginAsync(signIn);
        }
        public ResponseModel<List<AppUserListDto>> UserList(bool IsDeleted)
        {
            var result = _panelAppUserRepository.WhereForRead(w => w.IsDeleted == IsDeleted);

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
                    IsDefaultAdmin = s.IsDefaultAdmin,
                    TenantId = s.TenantId,
                    UserName = s.UserName
                }).ToList(),
                Message = new ResultMessage("Kullanıcı Listesi Başarıyla Getirildi"),
                Notification = Core.Enums.NotificationType.Success,
                Status = HttpStatusCode.OK
            };

        }
    }
}
