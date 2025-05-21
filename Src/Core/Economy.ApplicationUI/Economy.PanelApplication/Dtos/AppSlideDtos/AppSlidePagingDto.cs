namespace Economy.Panel.Application.Dtos.AppSlideDtos
{
    public class AppSlidePagingDto
    {
        public int Count { get; set; }
        public AppSlideDto Items { get; set; } = new();
    }
}
