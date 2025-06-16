using Economy.Base.Application.Dtos.BaseModels;
using Economy.Core.Dtos;
using Economy.Core.Tools;

namespace Economy.Panel.Application.Interfaces
{
    public interface IPanelAppUserService
    {
        Task<ResponseModel<Token>> LoginAsync(SignIn signIn);
        Task<ResponseModel<AppUserDto>> CreateUser(AppUserCreateDto userCreateDto);
        ResponseModel<List<AppUserListDto>> UserList(bool IsDeleted);
        ResponseModel<AppUserDto> EditUser(AppUserEditDto userEditDto);
        ResponseModel<AppUserDto> GetUser(int id, bool isDeleted);
        ResponseModel<AppUserDto> DeleteUser(int Id);


        //Task<ResponseModel<AppUserDto>> CreateUser(AppUserCreateDto userCreateDto);
        //Task<ResponseModel<Token>> LoginAsync(SignIn signIn);

    }
}
