using Economy.Core.Dtos;
using Economy.Core.Tools;
using Economy.Panel.Application.Interfaces;
using Economy.Panel.Application.Repositories;

namespace Economy.Panel.Persistence.Services
{
    public class PanelAppUserService : IPanelAppUserService
    {
        private readonly PanelAppUserRepository _panelAppUserRepository;

        public PanelAppUserService(PanelAppUserRepository panelAppUserRepository)
        {
            _panelAppUserRepository = panelAppUserRepository;
        }

        public async Task<ResponseModel<Token>> LoginAsync(SignIn signIn)
        {
            return await _panelAppUserRepository.LoginAsync(signIn);
        }
    }
}
