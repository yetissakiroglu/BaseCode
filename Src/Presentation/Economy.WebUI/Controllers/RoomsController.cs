namespace Economy.Web.UI.Controllers
{
    using Economy.Web.UI.Services.Abstractions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.RateLimiting;

    public class RoomsController : Controller
    {
        private readonly IContentService _content; private readonly IReservationService _rez; private readonly IRecaptchaService _rc; private readonly ICommentsService _comments;
        public RoomsController(IContentService c, IReservationService r, IRecaptchaService rc, ICommentsService cm) { _content = c; _rez = r; _rc = rc; _comments = cm; }

        public async Task<IActionResult> Index()
        {
            var c = Thread.CurrentThread.CurrentUICulture.Name;
            var list = await _content.GetRoomsAsync(c);
            return View(list);
        }

        [Route("{culture?}/rooms/{slug}")]
        public async Task<IActionResult> Detail(string slug)
        {
            var c = Thread.CurrentThread.CurrentUICulture.Name;
            var room = await _content.GetRoomAsync(c, slug);
            if (room is null) return NotFound();
            return View(room);
        }

        [EnableRateLimiting("forms")]
        [ValidateAntiForgeryToken, HttpPost]
        public async Task<IActionResult> Reserve(ReservationRequestDto model, string recaptchaToken)
        {
            if (!await _rc.VerifyAsync(recaptchaToken)) return BadRequest(new { ok = false });
            var (ok, refno) = await _rez.CreateAsync(model);
            return Ok(new { ok, reference = refno });
        }
    }

}
