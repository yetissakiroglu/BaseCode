using Economy.Core.Interfaces;
using Economy.Domain.Entites.EntityAppUsers;

namespace Economy.Application.Repositories.UserServiceRepositories
{
	public interface IRefreshTokenRepository : IEntityRepository<AppUserRefreshToken, string>
	{



	}
}
