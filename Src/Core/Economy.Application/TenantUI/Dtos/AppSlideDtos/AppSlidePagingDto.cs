namespace Economy.Application.TenantUI.Dtos.AppSlideDtos
{
    public class AppSlidePagingDto
    {
        public int Count { get; set; }
        public AppSlideDto Items { get; set; } = new();
    }
}
