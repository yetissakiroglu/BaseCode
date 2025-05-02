using Economy.Core.Dtos;
using Economy.Core.Tools;

namespace Economy.Panel.Application.Interfaces
{
    public interface IPanelAppUserService
    {
        Task<ResponseModel<Token>> LoginAsync(SignIn signIn);

    }
}
