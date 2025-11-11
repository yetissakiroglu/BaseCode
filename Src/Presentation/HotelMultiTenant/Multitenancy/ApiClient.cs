using System.Text;
using System.Text.Json;

namespace HotelMultiTenant.Multitenancy
{
    public sealed class ApiClient : IApiClient
    {
        private readonly IHttpClientFactory _factory;
        private readonly IHttpContextAccessor _ctx;
        private static readonly JsonSerializerOptions J = new(JsonSerializerDefaults.Web);

        public ApiClient(IHttpClientFactory factory, IHttpContextAccessor ctx)
        {
            _factory = factory;
            _ctx = ctx;
        }

        private HttpRequestMessage Create(HttpMethod method, string url, object? body)
        {
            var req = new HttpRequestMessage(method, url);

            var tenantKey = _ctx.HttpContext?.Request.Headers["X-TENANT"].FirstOrDefault()
                            ?? _ctx.HttpContext?.Request.Host.Host;
            if (!string.IsNullOrWhiteSpace(tenantKey))
                req.Headers.Add("X-TENANT", tenantKey);

            if (body is not null)
            {
                var json = JsonSerializer.Serialize(body, J);
                req.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            return req;
        }

        private async Task<T?> SendAsync<T>(HttpMethod method, string url, object? body, CancellationToken ct)
        {
            var client = _factory.CreateClient("api");
            using var req = Create(method, url, body);
            using var res = await client.SendAsync(req, ct);
            res.EnsureSuccessStatusCode();

            if (typeof(T) == typeof(bool))
                return (T?)(object)true;

            await using var s = await res.Content.ReadAsStreamAsync(ct);
            return await JsonSerializer.DeserializeAsync<T>(s, J, ct);
        }

        public Task<T?> GetAsync<T>(string url, CancellationToken ct = default)
            => SendAsync<T>(HttpMethod.Get, url, null, ct);

        public Task<T?> PostAsync<T>(string url, object body, CancellationToken ct = default)
            => SendAsync<T>(HttpMethod.Post, url, body, ct);

        public Task<T?> PutAsync<T>(string url, object body, CancellationToken ct = default)
            => SendAsync<T>(HttpMethod.Put, url, body, ct);

        public Task<bool> DeleteAsync(string url, CancellationToken ct = default)
            => SendAsync<bool>(HttpMethod.Delete, url, null, ct)!;
    }
}
