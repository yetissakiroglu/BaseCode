using Economy.Web.UI.Services.Abstractions;

namespace Economy.Web.UI.Services.Implementations
{
    public class InMemoryFaqService : IFaqService
    {
        private readonly List<FaqItem> _faqs = new()
    {
        new(1,"tr-TR","Check-in saati?","14:00 sonrası", new[]{"checkin","giriş"}),
        new(2,"en-US","Check-in time?","After 2pm", new[]{"checkin","time"}),
    };

        public Task<List<FaqItem>> SearchAsync(string culture, string? q = null)
        {
            var res = _faqs.Where(f => f.Culture == culture);
            if (!string.IsNullOrWhiteSpace(q))
                res = res.Where(f => f.Question.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                                     f.Answer.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                                     f.Keywords.Any(k => k.Contains(q, StringComparison.OrdinalIgnoreCase)));
            return Task.FromResult(res.OrderBy(f => f.Id).ToList());
        }
    }

}
