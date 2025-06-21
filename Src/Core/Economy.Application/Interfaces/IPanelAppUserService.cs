using Economy.Base.Application.Dtos.BaseModels;
using Economy.Core.Dtos;
using Economy.Core.Tools;
using Economy.Core.Tools.Result;

namespace Economy.Panel.Application.Interfaces
{
    public interface IPanelAppUserService
    {
        Task<ServiceResult<AppUserDto>> CreateUser(AppUserCreateDto model);



        Task<ResponseModel<Token>> LoginAsync(SignIn signIn);
        ResponseModel<List<AppUserListDto>> UserList(bool IsDeleted);
        ResponseModel<AppUserDto> EditUser(AppUserEditDto userEditDto);
        ResponseModel<AppUserDto> GetUser(int id, bool isDeleted);
        ResponseModel<AppUserDto> DeleteUser(int Id);


        //Task<ResponseModel<AppUserDto>> CreateUser(AppUserCreateDto userCreateDto);
        //Task<ResponseModel<Token>> LoginAsync(SignIn signIn);

    }
}
