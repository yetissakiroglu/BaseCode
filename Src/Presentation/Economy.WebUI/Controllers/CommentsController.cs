namespace Economy.Web.UI.Controllers
{
    using Economy.Web.UI.Services.Abstractions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.RateLimiting;

    public class CommentsController : Controller
    {
        private readonly ICommentsService _svc; private readonly IRecaptchaService _rc;
        public CommentsController(ICommentsService s, IRecaptchaService rc) { _svc = s; _rc = rc; }

        [HttpGet]
        public async Task<IActionResult> List(string type, string slug)
        {
            var c = Thread.CurrentThread.CurrentUICulture.Name;
            var list = await _svc.ListAsync(c, type, slug);
            return PartialView("~/Views/Shared/_CommentsList.cshtml", list);
        }

        [EnableRateLimiting("comments")]
        [ValidateAntiForgeryToken, HttpPost]
        public async Task<IActionResult> Create(string type, string slug, string name, string text, int rating, string recaptchaToken)
        {
            if (!await _rc.VerifyAsync(recaptchaToken)) return BadRequest(new { ok = false });
            var c = Thread.CurrentThread.CurrentUICulture.Name;
            await _svc.CreateAsync(new Comment(0, c, type, slug, name, text, rating, DateTime.UtcNow));
            return Ok(new { ok = true });
        }
    }

}
