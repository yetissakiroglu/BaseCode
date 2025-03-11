using System.Globalization;

namespace AppWeb.Providers
{
    public abstract class LanguageProvider
    {
        // Soyut metod, kullanıcının dil tercihini döndüren
        public abstract string GetCurrentLanguage();
    }

    public class UserLanguageProvider : LanguageProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserLanguageProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Kullanıcı dil bilgisini Session üzerinden alıyoruz
        public override string GetCurrentLanguage()
        {
            var lang = _httpContextAccessor.HttpContext?.Request.Cookies["UserLanguage"];

            if (string.IsNullOrEmpty(lang))
            {
                // Eğer kullanıcı dil tercihi yoksa, varsayılan kültürü kullanıyoruz
                lang = CultureInfo.CurrentUICulture.Name;
            }

            return lang;
        }
    }
}
