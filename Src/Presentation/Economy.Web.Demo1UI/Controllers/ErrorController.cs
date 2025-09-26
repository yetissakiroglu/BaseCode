using Microsoft.AspNetCore.Mvc;

public class ErrorController : Controller
{
    [Route("Error/Server")]
    public IActionResult Server()
    {
        Response.Headers["X-Robots-Tag"] = "noindex, nofollow";
        return View("Server");
    }

    [Route("Error/Status/{code}")]
    public IActionResult Status(int code)
    {
        if (code == 404) Response.Headers["X-Robots-Tag"] = "noindex, nofollow";
        return View("Status", code);
    }
}
