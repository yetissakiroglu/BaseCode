using System.Net.Http.Headers;
using System.Text.Json;

namespace Economy.Web.Demo1.Helpers
{
    public class ApiClientHelper : IApiClientHelper
    {
        private readonly IHttpClientFactory _httpFactory;

        public ApiClientHelper(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }

        public async Task<T?> GetAsync<T>(string url, string? lang = "tr", CancellationToken ct = default)
        {
            var client = _httpFactory.CreateClient("ApiClient");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(lang))
            {
                client.DefaultRequestHeaders.AcceptLanguage.Clear();
                client.DefaultRequestHeaders.AcceptLanguage.ParseAdd(lang);
            }

            using var res = await client.GetAsync(url, ct);
            res.EnsureSuccessStatusCode();

            await using var stream = await res.Content.ReadAsStreamAsync(ct);
            return await JsonSerializer.DeserializeAsync<T>(stream,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, ct);
        }
    }

}
