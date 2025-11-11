namespace HotelMultiTenant.Multitenancy
{
    public interface IApiClient
    {
        Task<T?> GetAsync<T>(string url, CancellationToken ct = default);
        Task<T?> PostAsync<T>(string url, object body, CancellationToken ct = default);
        Task<T?> PutAsync<T>(string url, object body, CancellationToken ct = default);
        Task<bool> DeleteAsync(string url, CancellationToken ct = default);
    }
}
