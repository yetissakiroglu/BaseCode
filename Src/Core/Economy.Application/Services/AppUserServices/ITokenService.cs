using Core.Models.Business;
using Core.Models.Dto;
using Economy.Domain.Entites.Identities;

namespace Economy.Application.Services.AppUserServices
{
	public interface ITokenService
	{
		Token CreateToken(AppUser user);
		ClientToken CreateToken(Client client);
	}
}
