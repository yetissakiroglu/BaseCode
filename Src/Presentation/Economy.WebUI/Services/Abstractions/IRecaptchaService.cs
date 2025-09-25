namespace Economy.Web.UI.Services.Abstractions
{
    public interface IRecaptchaService
    {
        Task<bool> VerifyAsync(string token);
        string? GetSiteKey();
    }

}
