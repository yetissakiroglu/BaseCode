using Economy.Application.Repositories.UserServiceRepositories;
using Economy.Domain.Entites.EntityAppUsers;
using Economy.Persistence.Contexts;
using Economy.Persistence.Repositories.AppBase.EntityFramework;

namespace Economy.Persistence.Repositories.UserServiceRepositories
{
	public class RefreshTokenRepository(AppDbContext context) : EfEntityRepositoryBase<UserRefreshToken, string>(context), IRefreshTokenRepository
	{




	}
}