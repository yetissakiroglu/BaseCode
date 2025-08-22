using Economy.Application.Interfaces;
using Economy.Core.Interfaces;
using Economy.Domain.Entites.AppEntities;
using Economy.Domain.Entites.Identities;
using Microsoft.AspNetCore.Http;

namespace Economy.Persistence.Services
{
    public class AuditLogWriter : IAuditLogWriter
    {
        private readonly IUnitOfWork _uow;
        private readonly IEntityRepository<AppAuditLog, int> _repo;

        public AuditLogWriter(IUnitOfWork uow)
        {
            _uow = uow;
            _repo = uow.DefaultEntityRepository<AppAuditLog>();
        }

        public async Task LogLoginAsync(AppUser? user, string userName, bool succeeded, string reason, HttpContext http, int? statusCode = null)
        {
            var log = new AppAuditLog
            {
                CreatedDate = DateTime.UtcNow,          // Entity’ndeki alan
                UserId = user?.Id,
                UserName = user?.UserName ?? userName,
                Action = "Login",
                Succeeded = succeeded,
                StatusCode = statusCode ?? (succeeded ? 200 : 401),
                Message = Trunc(reason, 512),
                HttpMethod = http.Request.Method,
                RequestPath = http.Request.Path.Value,
                IpAddress = GetClientIp(http),
                UserAgent = http.Request.Headers.UserAgent.ToString(),
                CorrelationId = http.TraceIdentifier
            };

            _repo.Add(log);
            await _uow.SaveDefaultChangesAsync();
        }

        private static string? Trunc(string? s, int max) =>
            string.IsNullOrEmpty(s) ? s : (s.Length <= max ? s : s[..max]);

        private static string? GetClientIp(HttpContext ctx)
        {
            // proxy arkasında X-Forwarded-For öncelikli
            if (ctx.Request.Headers.TryGetValue("X-Forwarded-For", out var v) && !string.IsNullOrWhiteSpace(v))
                return v.ToString().Split(',')[0].Trim();
            return ctx.Connection.RemoteIpAddress?.ToString();
        }
    }
}
