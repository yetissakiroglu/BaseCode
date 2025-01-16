using Economy.Application.Services.BaseServices;
using Economy.Domain.Entites;

namespace Economy.Application.Services
{
    public interface ISessionActivityServices : IService<SessionActivity,string>
    {
        Task RecordActivityLoginAsync(string email);
        Task RecordActivityLogoutAsync();
    }
}
