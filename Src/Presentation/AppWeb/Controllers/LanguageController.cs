using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace AppWeb.Controllers
{
    public class LanguageController : Controller
    {
        public IActionResult ChangeLanguage(string lang)
        {

            // Kültür bilgisini ayarlıyoruz
            var culture = new CultureInfo(lang);
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            // Dil değişikliğini cookie'ye kaydediyoruz
            Response.Cookies.Append("UserLanguage", lang);


            // Dil değişikliğinden sonra kullanıcıyı anasayfaya yönlendiriyoruz
            return RedirectToAction("Index", "Home");
            //}

            //// Dil parametresi yoksa varsayılan olarak Türkçe'yi seçiyoruz
            //return RedirectToAction("Index", "Home", new { lang = "tr" });
        }
    }
}
