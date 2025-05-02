using Economy.Application.Interfaces.AppUserServices;
using Economy.Base.Application.BaseRepositories;
using Economy.Core.Interfaces;
using Economy.Domain.Entites.Identities;
using Economy.Panel.Application.Repositories;
using Economy.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;

namespace Economy.Panel.Persistence.Repositories
{
    public class ConcretePanelAppUserRepository : PanelAppUserRepository
    {
        public ConcretePanelAppUserRepository(DefaultDbContext _context, UserManager<AppUser> userManager, ITokenService tokenService, IAppUserTokenBaseRepository appUserTokenBaseRepository, IUnitOfWork unitOfWork) : base(_context, userManager, tokenService, appUserTokenBaseRepository, unitOfWork)
        {
        }
    }
}
