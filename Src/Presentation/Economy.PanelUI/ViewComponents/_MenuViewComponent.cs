using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.ViewComponents
{
    public class _MenuViewComponent : ViewComponent
    {
        // Bu metod, kullanıcının giriş yapıp yapmadığını kontrol eder
        public IViewComponentResult Invoke()
        {

            // Kullanıcı giriş yapmamışsa, login formunu gösteriyoruz
            return View("Components/Header/_MenuComponent.cshtml");

        }
    }
}
