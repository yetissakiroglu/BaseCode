using Economy.Domain.Entites.EntityPages;

namespace Economy.Application.ApiDtos
{
    public record ResponseFeaturedApiDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public static ResponseFeaturedApiDto FromEntity(AppContent haber)
        {
            return new ResponseFeaturedApiDto
            {
                Id = haber.Id,
                Title = haber.Title,           
                Url = haber.GetUrl(),  
            };
        }
        public static List<ResponseFeaturedApiDto> FromEntities(List<AppContent> haberList)
        {
            return haberList.Select(haber => FromEntity(haber)).ToList();
        }
    }
}
