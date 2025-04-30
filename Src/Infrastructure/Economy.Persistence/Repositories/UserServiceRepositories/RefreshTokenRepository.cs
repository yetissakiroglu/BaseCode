using Economy.Application.Repositories.UserServiceRepositories;
using Economy.Domain.Entites.EntityAppUsers;
using Economy.Persistence.Contexts;
using Economy.Persistence.Repositories.AppBase.EntityFramework;

namespace Economy.Persistence.Repositories.UserServiceRepositories
{
	public class RefreshTokenRepository(DefaultDbContext context) : EfEntityRepositoryBase<AppUserRefreshToken, string>(context), IRefreshTokenRepository
	{




	}
}