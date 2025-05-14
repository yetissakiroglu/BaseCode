using Economy.Base.Application.BaseRepositories;
using Economy.Base.Application.Interfaces;
using Economy.Core.Interfaces;
using Economy.Core.Interfaces.Economy.Panel.Application.Repositories;
using Economy.Domain.Entites.Identities;
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
