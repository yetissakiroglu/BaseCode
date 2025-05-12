using Economy.Persistence.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Economy.Core.Services.Providers
{
    public class TenantProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DefaultDbContext _masterDbContext;
        private readonly int defultUserId = 2;
        public TenantProvider(IHttpContextAccessor httpContextAccessor, DefaultDbContext masterDbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _masterDbContext = masterDbContext;
        }

        public async Task<string> GetConnectionStringAsync()
        {
            //TODO burası sonra açılacak
            //var userIdStr = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            //if (userIdStr == null) throw new UnauthorizedAccessException("User not logged in.");

            //int userId = int.Parse(userIdStr);

            var user = await _masterDbContext.Users.FindAsync(defultUserId);
            if (user == null) throw new Exception("User not found.");

            var tenant = await _masterDbContext.Apps.FindAsync(user.TenantId);
            if (tenant == null) throw new Exception("Tenant not found.");

            return tenant.ConnectionString;
        }

        public async Task<List<string>> GetAllConnectionStringsAsync()
        {
            //var userIdStr = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            //if (userIdStr == null) throw new UnauthorizedAccessException("User not logged in.");

            //int userId = int.Parse(userIdStr);

            var user = await _masterDbContext.Users.FindAsync(defultUserId);
            if (user == null) throw new Exception("User not found.");

            var tenant = await _masterDbContext.Apps.Where(w=>!w.IsDeleted).Select(t => t.ConnectionString).ToListAsync();
            if (tenant == null) throw new Exception("Tenant not found.");

            return tenant;
        }


    }
}
