using Economy.Base.Application.Dtos.BaseModels;
using Economy.Core.Dtos;
using Economy.Core.Tools;
using Economy.Domain.Entites.Identities;

namespace Economy.Panel.Application.Interfaces
{
    public interface IPanelAppUserService
    {
        Task<ResponseModel<Token>> LoginAsync(SignIn signIn);
        Task<ResponseModel<AppUserDto>> CreateUser(AppUserCreateDto userCreateDto);
        ResponseModel<List<AppUserListDto>> UserList(bool IsDeleted);




    }
}
