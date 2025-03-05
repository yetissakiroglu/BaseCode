using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace AppWeb.Controllers
{
    public class LanguageController : Controller
    {
        //public IActionResult SetLanguage(string culture)
        //{
        //    if (!string.IsNullOrEmpty(culture))
        //    {
        //        Response.Cookies.Append("CurrentCulture", culture, new CookieOptions
        //        {
        //            Expires = DateTimeOffset.UtcNow.AddYears(1)
        //        });
        //    }

        //    return RedirectToAction("Index"); // Anasayfaya yönlendir
        //}

        public IActionResult ChangeLanguage(string lang)
        {     // Cookie'yi temizle
            Response.Cookies.Delete("CurrentCulture");  
            
            // Cookie'yi temizliyoruz
                                                                //// Eğer lang parametresi boş değilse, kültür bilgisini değiştiriyoruz
                                                                //if (!string.IsNullOrEmpty(lang))
                                                                //{
                                                                // Kültür bilgisini ayarlıyoruz
            var culture = new CultureInfo(lang);
                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;

                // Dil değişikliğini cookie'ye kaydediyoruz
                Response.Cookies.Append(
                    "CurrentCulture",
                    lang,
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddMinutes(1) });

                // Dil değişikliğinden sonra kullanıcıyı anasayfaya yönlendiriyoruz
                return RedirectToAction("Index", "Home");
            //}

            //// Dil parametresi yoksa varsayılan olarak Türkçe'yi seçiyoruz
            //return RedirectToAction("Index", "Home", new { lang = "tr" });
        }
    }
}
