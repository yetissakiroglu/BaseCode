namespace Economy.Web.UI.Services.Abstractions
{
    public record FaqItem(int Id, string Culture, string Question, string Answer, string[] Keywords);

    public interface IFaqService
    {
        Task<List<FaqItem>> SearchAsync(string culture, string? q = null);
    }

}
