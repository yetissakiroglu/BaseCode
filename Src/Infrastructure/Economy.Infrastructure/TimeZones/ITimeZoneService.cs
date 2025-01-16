using Microsoft.AspNetCore.Mvc.Rendering;

namespace Economy.Infrastructure.TimeZones
{
    public interface ITimeZoneService
    {
        ICollection<SelectListItem> GetAllTimeZones();
    }
}
