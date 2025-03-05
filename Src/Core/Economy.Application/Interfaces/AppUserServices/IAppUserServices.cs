using Economy.Core.Dtos;
using Economy.Core.Tools;

namespace Economy.Application.Interfaces.AppUserServices
{
    public interface IAppUserServices
    {
        Task<ResponseModel<Token>> CreateTokenAsync(SignIn signIn);
        Task<ResponseModel<Token>> RefreshTokenAsync(string refreshToken);
        Task<ResponseModel<string>> RevokeRefreshTokenAsync(string refreshToken);
        ResponseModel<ClientToken> CreateToken(ClientSignIn clientSignIn);

        ////-Bakılacak
        //Task<AppUser?> GetUndeletedUserAsync(string email);
        //Task DeleteAsync(string? id);
    }
}
