using Economy.Application.Interfaces.AppUserServices;
using Economy.Base.Application.BaseRepositories;
using Economy.Core.Interfaces;
using Economy.Domain.Entites.Identities;
using Economy.Persistence.BaseRepositories;
using Economy.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;

namespace Economy.Panel.Application.Repositories
{
    public abstract class PanelAppUserRepository : AppUserBaseRepository
    {
        public PanelAppUserRepository(DefaultDbContext _context, UserManager<AppUser> userManager, ITokenService tokenService, IAppUserTokenBaseRepository appUserTokenBaseRepository, IUnitOfWork unitOfWork) : base(_context, userManager, tokenService, appUserTokenBaseRepository, unitOfWork)
        {
        }
    }


}
