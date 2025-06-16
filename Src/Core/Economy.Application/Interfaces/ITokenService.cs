using Economy.Core.Business;
using Economy.Core.Dtos;
using Economy.Domain.Entites.Identities;

namespace Economy.Panel.Application.Interfaces
{
    public interface ITokenService
    {
        Token CreateToken(AppUser user);
        ClientToken CreateToken(Client client);
    }
}
