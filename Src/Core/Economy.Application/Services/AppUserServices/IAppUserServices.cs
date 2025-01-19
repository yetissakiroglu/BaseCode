using Core.Models.Dto;
using Economy.Persistence.Services;

namespace Economy.Application.Services.AppUserServices
{
    public interface IAppUserServices 
    {

		Task<ServiceResult<Token>> CreateTokenAsync(SignIn signIn);
		Task<ServiceResult<Token>> RefreshTokenAsync(string refreshToken);
		Task<ServiceResult<string>> RevokeRefreshTokenAsync(string refreshToken);
		ServiceResult<ClientToken> CreateToken(ClientSignIn clientSignIn);





		////-Bakılacak
		//Task<AppUser?> GetUndeletedUserAsync(string email);
		//Task DeleteAsync(string? id);
	}
}
