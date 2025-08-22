using Economy.Domain.Entites.Identities;
using Microsoft.AspNetCore.Http;

namespace Economy.Application.Interfaces
{
    public interface IAuditLogWriter
    {
        Task LogLoginAsync(AppUser? user, string userName, bool succeeded, string reason, HttpContext http, int? statusCode = null);
    }
}
