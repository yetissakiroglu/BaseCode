using Economy.UI.Dtos;
using HotelMultiTenant.Multitenancy;

namespace HotelMultiTenant.Services
{

    public class InMemoryTenantDirectory : ITenantDirectory
    {
        private readonly IContentService _contentService;

        public InMemoryTenantDirectory(IContentService contentService)
        {
            _contentService = contentService;
        }

        public async Task<TenantDto?> ResolveByHostAsync(string host, CancellationToken ct = default)
        {
            var t = await _contentService.GetTanentAsync(ct);
            return await Task.FromResult<TenantDto?>(t);
        }
    }
}
