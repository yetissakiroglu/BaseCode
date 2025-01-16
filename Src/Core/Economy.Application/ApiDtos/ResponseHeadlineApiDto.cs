using Economy.Domain.Entites.EntityPages;

namespace Economy.Application.ApiDtos
{
    public record ResponseHeadlineApiDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CategoryTitle { get; set; }
        public string CategoryUrl { get; set; }
        public string ShortDescription { get; set; }
        
        public string Url { get; set; }
        public static ResponseHeadlineApiDto FromEntity(AppContent haber)
        {
            return new ResponseHeadlineApiDto
            {
                Id = haber.Id,
                Title = haber.Title,
                CategoryTitle = haber.AppCategory?.Name,
                CategoryUrl= haber.AppCategory?.GetUrlPath(),
                Url = haber.GetUrl(),
                ShortDescription = haber.ShortDescription
            };
        }
        public static List<ResponseHeadlineApiDto> FromEntities(List<AppContent> haberList)
        {
            return haberList.Select(haber => FromEntity(haber)).ToList();
        }
    }
}
