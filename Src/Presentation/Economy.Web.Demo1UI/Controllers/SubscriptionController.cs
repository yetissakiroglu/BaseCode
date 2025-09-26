using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace MyHotelSite.Controllers;

[OutputCache(PolicyName = "LangAnon300")]
public class SubscriptionController : Controller
{
    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("{lang}/subscription/subscribe")]
    public IActionResult Subscribe(string lang, [FromForm] string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@")) { return BadRequest(); }
        return Redirect($"/{lang}/subscribe/thanks");
    }

    [Route("{lang}/subscribe/thanks")]
    public IActionResult Thanks(string lang = "tr") => View();
}
