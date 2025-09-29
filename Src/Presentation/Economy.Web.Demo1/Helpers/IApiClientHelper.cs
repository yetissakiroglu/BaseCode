namespace Economy.Web.Demo1.Helpers
{
    public interface IApiClientHelper
    {
        Task<T?> GetAsync<T>(string url, string? lang = "tr", CancellationToken ct = default);
    }
}
