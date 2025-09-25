namespace Economy.Web.UI.Services.Abstractions
{
    public record Comment(int Id, string Culture, string Type, string Slug, string Name, string Text, int Rating, DateTime CreatedAt);
    public interface ICommentsService
    {
        Task<List<Comment>> ListAsync(string culture, string type, string slug, int take = 50);
        Task<bool> CreateAsync(Comment c);
    }

}
