using Economy.Base.Application.Dtos.BaseModels;
using Economy.Core.Dtos;
using Economy.Core.Interfaces;
using Economy.Core.Tools;
using Economy.Domain.Entites.Identities;

namespace Economy.Application.BaseRepositories
{
    public interface IAppUserBaseRepository : IEntityRepository<AppUser, int>
    {
        Task<ResponseModel<AppUserDto>> CreateUser(AppUserCreateDto userCreateDto); 
        Task<ResponseModel<Token>> LoginAsync(SignIn signIn);       

    }

}
