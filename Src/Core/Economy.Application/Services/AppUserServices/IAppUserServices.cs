using Core.Models.Dto;
using Economy.Application.Services.BaseServices;
using Economy.Domain.Entites.Identities;
using Economy.Persistence.Services;

namespace Economy.Application.Services.AppUserServices
{
	public interface IAppUserServices : IService<AppUser, string>
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
