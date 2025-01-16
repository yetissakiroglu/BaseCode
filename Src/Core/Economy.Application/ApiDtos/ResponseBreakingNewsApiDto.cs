using Economy.Domain.Entites.EntityPages;

namespace Economy.Application.ApiDtos
{
    public record ResponseBreakingNewsApiDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public static ResponseBreakingNewsApiDto FromEntity(AppContent haber)
        {
            return new ResponseBreakingNewsApiDto
            {
                Id = haber.Id,
                Title = haber.Title,
                Url = haber.GetUrl(),
            };
        }
        public static List<ResponseBreakingNewsApiDto> FromEntities(List<AppContent> haberList)
        {
            return haberList.Select(haber => FromEntity(haber)).ToList();
        }
    }
}
