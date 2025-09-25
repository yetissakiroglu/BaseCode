using Economy.Web.UI.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Web.UI.ViewComponents
{
    public class LanguageSwitcherViewComponent : ViewComponent
    {
        private readonly ILanguageService _langs;
        public LanguageSwitcherViewComponent(ILanguageService langs) => _langs = langs;
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = await _langs.GetAsync();
            ViewBag.Current = Thread.CurrentThread.CurrentUICulture.Name;
            return View(list);
        }
    }

}
