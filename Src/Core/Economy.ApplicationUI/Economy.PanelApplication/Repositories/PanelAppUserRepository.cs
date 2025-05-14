using Economy.Base.Application.BaseRepositories;
using Economy.Base.Application.Interfaces;
using Economy.Domain.Entites.Identities;
using Economy.Persistence.BaseRepositories;
using Economy.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;

namespace Economy.Core.Interfaces.Economy.Panel.Application.Repositories
{
    public abstract class PanelAppUserRepository : AppUserBaseRepository
    {
        public PanelAppUserRepository(DefaultDbContext _context, UserManager<AppUser> userManager, ITokenService tokenService, IAppUserTokenBaseRepository appUserTokenBaseRepository, IUnitOfWork unitOfWork) : base(_context, userManager, tokenService, appUserTokenBaseRepository, unitOfWork)
        {
        }
    }


}
